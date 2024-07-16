namespace RPSSL.Web.Contracts._Common.Response;

public enum ErrorCodes
{
    UnspecifiedError = 0,
    DatabaseAdd,
    DatabaseUpdate,
    DatabaseGet,
    CantFind,
    NotAllowed,
    AlreadyExists,
    FileProblem,
    Authorization,
    TokenParse,
    ObjectParsing
}
