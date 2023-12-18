using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [MaxLength(50, ErrorMessage = "Max Length of Name is 50 Char")]
        [MinLength(5, ErrorMessage = "Min Length of Name is 5 Char")]
        public string Name { get; set; }
        [Range(22, 45, ErrorMessage = "Ranage Age is between 22,45")]
        public int? Age { get; set; }

        public string Address { get; set; }

        

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public int? DepartmentId { get; set; }//Foregin Key

        public Department Department { get; set; }

    }
}
