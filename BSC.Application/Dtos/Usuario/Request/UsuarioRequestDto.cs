﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BSC.Application.Dtos.Usuario.Request
{
    public class UsuarioRequestDto
    {
        [Required]
        public string? NombreUsuario { get; set; }
        [Required]
        public string? Contrasena { get; set; }
        [Required]
        public string? Correo { get; set; }
        public string? Imagen { get; set; } = string.Empty;
        public string? TipoAutenticacion { get; set; } = "local";
        public int? Estado { get; set; } = 1;
    }
}