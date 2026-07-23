using CryptoExchange.Net.Objects.Errors;

namespace Pionex.Net
{
    internal static class PionexErrors
    {
        public static ErrorMapping Errors { get; } = new ErrorMapping(
            [
                new ErrorInfo(ErrorType.Unauthorized, false, "IP not whitelisted", "IP_NOT_WHITELISTED"),
                new ErrorInfo(ErrorType.Unauthorized, false, "Invalid credentials", "INVALID_APIKEY", "INVALID_SIGNATURE", "APIKEY_EXPIRED"),
                new ErrorInfo(ErrorType.Unauthorized, false, "Permission denied", "PERMISSION_DENIED"),

                new ErrorInfo(ErrorType.InvalidTimestamp, false, "Invalid timestamp", "INVALID_TIMESTAMP"),

                new ErrorInfo(ErrorType.UnknownSymbol, false, "Invalid symbol", "MARKET_INVALID_SYMBOL"),

            ]
            );
    }
}
