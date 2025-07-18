﻿namespace UserManagement.Shared.Models
{
    public class UserDto
    {
        public long Id { get; set; }
        public string? Forename { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; }
    }
}
