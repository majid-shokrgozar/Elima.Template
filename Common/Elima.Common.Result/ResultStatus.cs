namespace Elima.Common.Results
{
    public enum ResultStatus : int
    {
        //------
        Succeeded = 200,
        //------
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        //-------
        Failed = 500,
        NotImplemented = 501,
        Unprocessable= 422
    }
}
