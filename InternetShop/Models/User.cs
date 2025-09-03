using System;
using System.Collections.Generic;

namespace InternetShop.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public int RoleId { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly Birthday { get; set; }
}
