using Elima.Common.EntityFramework.EntityFrameworkCore.Modeling;
using Elima.Template.FirstModule.Domain.Samples;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elima.Template.FirstModule.Persistence.Samples
{
    internal class SampleEntityTypeConfiguration : IEntityTypeConfiguration<Sample>
    {
        public void Configure(EntityTypeBuilder<Sample> builder)
        {

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasConversion(
                sampleId => sampleId.Value,
                value => new SampleId(value)
                );

            builder.Property(x => x.Name).HasMaxLength(SampleConsts.NameMaxLength);


        }
    }
}
