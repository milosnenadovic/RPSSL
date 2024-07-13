namespace RPSSL.GameService.Common.Response;

public enum ErrorCodes
{
	UnspecifiedError = 0,
	DatabaseAdd,
	DatabaseUpdate,
	DatabaseGet,
	CantFind,
	NotAllowed,
	AlreadyExists,
	Authorization,
	TokenParse,
	ObjectParsing
}
