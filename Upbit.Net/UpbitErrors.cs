using CryptoExchange.Net.Objects.Errors;

namespace Upbit.Net
{
    internal static class UpbitErrors
    {
        public static ErrorMapping Errors { get; } = new ErrorMapping(
            [
                new ErrorInfo(ErrorType.UnknownSymbol, false, "Unknown symbol", "404")
            ]
            );
    }
}
