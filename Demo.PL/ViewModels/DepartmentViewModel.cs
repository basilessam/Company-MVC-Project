using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Demo.PL.ViewModels
{
    public class DepartmentViewModel
    {

		public int Id { get; set; }
		[Required(ErrorMessage = "Code is Required")]
		public string Code { get; set; }
		[Required(ErrorMessage = "Name is Required")]
		[MaxLength(50)]
		public string Name { get; set; }
		public DateTime DateOfCreation { get; set; }

		public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

	}
}
