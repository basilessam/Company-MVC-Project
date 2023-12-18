using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUintOfWork _uintOfWork;

        public DepartmentController(IMapper mapper, IUintOfWork uintOfWork)
        {
            _mapper = mapper;
            _uintOfWork = uintOfWork;
        }
        
        public async Task <IActionResult> Index()
        {
            var departments = await _uintOfWork.DepartmentRepository.GetAll();
            var mappedDeps =  _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedDeps); 
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentV)
        {
            if(ModelState.IsValid)
            {
                var mappedDep = _mapper.Map<DepartmentViewModel, Department>(departmentV);
                await _uintOfWork.DepartmentRepository.Add(mappedDep);
                await _uintOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(departmentV);
        }

        public async Task<IActionResult> Details(int? id ,string viewName= "Details")
        {
            if(id is null)
                return BadRequest();
            
            var department = await _uintOfWork.DepartmentRepository.Get(id.Value);
            await _uintOfWork.Complete();

            if (department is null)
                return NotFound();

            var mappedDep = _mapper.Map<Department, DepartmentViewModel>(department);

            return View(viewName, mappedDep);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [FromRoute] int id, DepartmentViewModel departmentV)
        {
            if (id != departmentV.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDep = _mapper.Map<DepartmentViewModel, Department>(departmentV);
                    _uintOfWork.DepartmentRepository.Update(mappedDep);
                   await _uintOfWork.Complete();
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(departmentV);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");

        }

        [HttpPost]

        public async Task<IActionResult> Delete([FromRoute] int id, DepartmentViewModel departmentV)
        {

            if (id != departmentV.Id)
            {
                return BadRequest();
            }
            try
            {
                var mappedDep = _mapper.Map<DepartmentViewModel, Department>(departmentV);

                _uintOfWork.DepartmentRepository.Delete(mappedDep);
                await _uintOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return View(departmentV);
            }
        }

    }
}
