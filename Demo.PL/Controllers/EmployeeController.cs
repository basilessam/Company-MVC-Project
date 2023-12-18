using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    //[Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;

        public EmployeeController
            (
            
            IUintOfWork uintOfWork
            ,IMapper mapper
            )
        {
           
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee>employees;

            if (string.IsNullOrEmpty(SearchValue))
            {
                 employees = await _uintOfWork.EmployeeRepository.GetAll();
            }
            else
            { 
                employees = _uintOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);
            }

            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(mappedEmps);
            
        }

        [HttpGet]
        public IActionResult Create()
        {
          //ViewBag.Department = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {


                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "images");

                var mappedEmp=_mapper.Map<EmployeeViewModel,Employee>( employeeVM );

                await _uintOfWork.EmployeeRepository.Add(mappedEmp);


                int count = await _uintOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }

        public async Task <IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var employee = await _uintOfWork.EmployeeRepository.Get(id.Value);

            if (employee is null)
                return NotFound();

            var mappedEmp=_mapper.Map<Employee, EmployeeViewModel>(employee);

            await _uintOfWork.Complete();

            return View(viewName, mappedEmp);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            
            if (id != employeeVM.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _uintOfWork.EmployeeRepository.Update(mappedEmp);
                    int count=await _uintOfWork.Complete();
                    if(count > 0&&employeeVM.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(employeeVM.ImageName, "images");
                    }
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            await _uintOfWork.Complete();

            return View(employeeVM);
        }

        public Task<IActionResult> Delete(int? id)
        {
            return Details(id, "Delete");

        }

        [HttpPost]

        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {

            if (id != employeeVM.Id)
            {
                return BadRequest();
            }
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                 _uintOfWork.EmployeeRepository.Delete(mappedEmp);
                await _uintOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }
            
        }
    }
}
