﻿using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class RegisterViewModel
	{
        public string FName { get; set; }
        public string LName { get; set; }
        [Required (ErrorMessage ="Email is required")]
		[EmailAddress (ErrorMessage ="Invaild Email")]

        public string  Email { get; set; }

		[Required(ErrorMessage ="Password is required")]
		[DataType (DataType.Password)]
        public string Password { get; set; }

		[Required(ErrorMessage ="Confrim Password is required")]
		[Compare("Password",ErrorMessage ="Confirm Password does not match Password")]
		[DataType (DataType.Password)]

		public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
		

    }
}
