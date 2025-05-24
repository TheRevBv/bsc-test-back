namespace BSC.Application.Dtos.SqlCore.Reponse
{
    public class SqlCoreResponseDto
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Los datos devueltos (puede ser IEnumerable<dynamic>, int, object, etc.).
        /// </summary>
        public object Data { get; set; } = null!;

        public static SqlCoreResponseDto Success(object data = null)
            => new SqlCoreResponseDto { IsSuccess = true, Data = data };

        public static SqlCoreResponseDto Failure(string message)
            => new SqlCoreResponseDto { IsSuccess = false, ErrorMessage = message };
    }
}


