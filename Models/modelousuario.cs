using System.ComponentModel.DataAnnotations;

namespace Gestper.Models;

public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio")]
    public string Apellido { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Debe ingresar un correo válido")]
    public string Correo { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Contrasena { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe tener exactamente 9 dígitos numéricos")]
    public string Telefono { get; set; }

    public int IdRol { get; set; }  // Este será 3 por defecto al registrar

    public bool Activo { get; set; }
    
    public ICollection<Ticket> TicketsAsignados { get; set; } = new List<Ticket>();
}