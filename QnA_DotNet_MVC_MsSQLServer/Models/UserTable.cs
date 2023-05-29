using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QnA_DotNet_MVC_MsSQLServer.Models
{
    [Table("UserTable")]
    public partial class UserTable
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        [StringLength(255)]
        public string Username { get; set; } = null!;
        [Column("password")]
        [StringLength(255)]
        public string Password { get; set; } = null!;
        [Column("firstName")]
        [StringLength(255)]
        public string FirstName { get; set; } = null!;
        [Column("lastName")]
        [StringLength(255)]
        public string LastName { get; set; } = null!;
        [Column("residence")]
        [StringLength(255)]
        public string Residence { get; set; } = null!;
        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; } = null!;
        [Column("dateOfBirth", TypeName = "datetime")]
        public DateTime DateOfBirth { get; set; }
        [Column("dateOfEntry", TypeName = "datetime")]
        public DateTime DateOfEntry { get; set; }
        [Column("dateOfDataEdit", TypeName = "datetime")]
        public DateTime DateOfDataEdit { get; set; }
        [Column("accessLevel")]
        public int AccessLevel { get; set; }
    }
}
