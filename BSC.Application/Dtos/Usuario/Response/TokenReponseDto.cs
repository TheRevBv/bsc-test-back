namespace BSC.Application.Dtos.Usuario.Response
{
    public class TokenReponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public DateTime Expiration { get; set; } = DateTime.Now;
    }
}
