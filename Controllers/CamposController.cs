﻿using Microsoft.AspNetCore.Mvc;
using Entidades.Services;
using Entidades.Models;

namespace Entidades.Controllers
{
    public class CamposController : Controller
    {

        private readonly CampoService _campoSerivce;
        public CamposController(CampoService campoService)
        {
            _campoSerivce = campoService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { Data = _campoSerivce.GetAll() });
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Campo campo)
        {
            if (ModelState.IsValid)
            {
                _campoSerivce.Create(campo);
                return RedirectToAction(nameof(Index));
            }
            return View(campo);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var campo = _campoSerivce.GetById(id);
            if (campo == null)
            {
                return NotFound();
            }
            return View(campo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Campo campo)
        {
            if (ModelState.IsValid)
            {
                _campoSerivce.Update(campo);
                return RedirectToAction(nameof(Index));
            }
            return View(campo);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDeb = _campoSerivce.GetById(id);

            if (objFromDeb == null)
            {
                return Json(new { success = false, message = "Error borrando campo" });
            }
            _campoSerivce.Delete(id);

            return Json(new { success = true, message = "Campo borrada" });
        }

    }
}
