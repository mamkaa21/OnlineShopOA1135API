using System;
using System.Collections.Generic;

namespace OnlineShopOA1135API;

public partial class User
{
    public int Id { get; set; }
    public int? RoleId { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual Role? Role { get; set; }
}
public partial class UserModel
{
    public int Id { get; set; }
    public int? RoleId { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public static explicit operator User(UserModel model)
    {
        return new User
        {

            Id = model.Id,
            Login = model.Login,
            Password = model.Password,
            RoleId = model.RoleId,
            Name = model.Name,
            Email = model.Email
        };
    }

    public static explicit operator UserModel(User model)
    {
        return new UserModel
        {

            Id = model.Id,
            Login = model.Login,
            Password = model.Password,
            RoleId = model.RoleId,
            Name = model.Name,
            Email = model.Email
        };
    }
}

