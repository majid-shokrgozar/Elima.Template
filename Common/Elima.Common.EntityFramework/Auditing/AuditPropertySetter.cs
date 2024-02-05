using Elima.Common.DependencyInjection;
using Elima.Common.Domain.Entities.Auditing.Contracts;
using Elima.Common.Security.Authentication;
using Microsoft.VisualBasic;
using System;

namespace Elima.Common.EntityFramework.Auditing;

public class AuditPropertySetter : IAuditPropertySetter, ITransientDependency
{
    protected ICurrentUser CurrentUser { get; }

    public AuditPropertySetter(
        ICurrentUser currentUser)
    {
        CurrentUser = currentUser;
    }

    public virtual void SetCreationProperties(object targetObject)
    {
        SetCreationTime(targetObject);
        SetCreatorId(targetObject);
    }

    public virtual void SetModificationProperties(object targetObject)
    {
        SetLastModificationTime(targetObject);
        SetLastModifierId(targetObject);
    }

    public virtual void SetDeletionProperties(object targetObject)
    {
        SetDeletionTime(targetObject);
        SetDeleterId(targetObject);
    }

    public virtual void IncrementEntityVersionProperty(object targetObject)
    {
        if (targetObject is IHasEntityVersion objectWithEntityVersion)
        {
            ObjectHelper.TrySetProperty(objectWithEntityVersion, x => x.EntityVersion, x => x.EntityVersion + 1);
        }
    }

    protected virtual void SetCreationTime(object targetObject)
    {
        if (!(targetObject is IHasCreationTime objectWithCreationTime))
        {
            return;
        }

        if (objectWithCreationTime.CreationTime == default)
        {
            ObjectHelper.TrySetProperty(objectWithCreationTime, x => x.CreationTime, () => DateAndTime.Now);
        }
    }

    protected virtual void SetCreatorId(object targetObject)
    {
        if (!CurrentUser.HasUserId)
        {
            return;
        }

        /* TODO: The code below is from old ABP, not implemented yet
            if (tenantId.HasValue && MultiTenancyHelper.IsHostEntity(entity))
            {
                //Tenant user created a host entity
                return;
            }
             */

        if (targetObject is IMayHaveCreator mayHaveCreatorObject)
        {
            if (!string.IsNullOrWhiteSpace(mayHaveCreatorObject.CreatorId))
            {
                return;
            }

            ObjectHelper.TrySetProperty(mayHaveCreatorObject, x => x.CreatorId, () => CurrentUser.Id);
        }
        else if (targetObject is IMustHaveCreator mustHaveCreatorObject)
        {
            if (mustHaveCreatorObject.CreatorId != default)
            {
                return;
            }

            ObjectHelper.TrySetProperty(mustHaveCreatorObject, x => x.CreatorId, () => CurrentUser.Id);
        }
    }

    protected virtual void SetLastModificationTime(object targetObject)
    {
        if (targetObject is IHasModificationTime objectWithModificationTime)
        {
            ObjectHelper.TrySetProperty(objectWithModificationTime, x => x.LastModificationTime, () => DateTime.Now);
        }
    }

    protected virtual void SetLastModifierId(object targetObject)
    {
        if (!(targetObject is IModificationAuditedObject modificationAuditedObject))
        {
            return;
        }

        if (!CurrentUser.HasUserId)
        {
            ObjectHelper.TrySetProperty(modificationAuditedObject, x => x.LastModifierId, () => null);
            return;
        }
        /* TODO: The code below is from old ABP, not implemented yet
        if (tenantId.HasValue && MultiTenancyHelper.IsHostEntity(entity))
        {
            //Tenant user modified a host entity
            modificationAuditedObject.LastModifierId = null;
            return;
        }
         */

        ObjectHelper.TrySetProperty(modificationAuditedObject, x => x.LastModifierId, () => CurrentUser.Id);
    }

    protected virtual void SetDeletionTime(object targetObject)
    {
        if (targetObject is IHasDeletionTime objectWithDeletionTime)
        {
            if (objectWithDeletionTime.DeletionTime == null)
            {
                ObjectHelper.TrySetProperty(objectWithDeletionTime, x => x.DeletionTime, () => DateTime.Now);
            }
        }
    }

    protected virtual void SetDeleterId(object targetObject)
    {
        if (!(targetObject is IDeletionAuditedObject deletionAuditedObject))
        {
            return;
        }

        if (deletionAuditedObject.DeleterId != null)
        {
            return;
        }

        if (!CurrentUser.HasUserId)
        {
            ObjectHelper.TrySetProperty(deletionAuditedObject, x => x.DeleterId, () => null);
            return;
        }

        ObjectHelper.TrySetProperty(deletionAuditedObject, x => x.DeleterId, () => CurrentUser.Id);
    }
}
