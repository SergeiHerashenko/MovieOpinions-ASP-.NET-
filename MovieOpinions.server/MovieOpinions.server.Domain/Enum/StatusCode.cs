namespace MovieOpinions.server.Domain.Enum
{
    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        Forbidden = 403,
        Gone = 410,
        Conflict = 409
    }
}
