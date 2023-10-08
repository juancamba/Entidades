﻿using Entidades.Models;
using Entidades.Models.DTO;
using Entidades.Models.ViewModels;
using Entidades.Repositories;
using Entidades.Serivces.Ficheros;
using Entidades.Services;
using Microsoft.AspNetCore.Mvc;
using static Entidades.Models.Enum;

namespace Entidades.Controllers
{
    public class EntidadesController : BaseController
    {

        private readonly AppDbContext _entidadesContext;
        private readonly IEntidadesRepository _entidadesRepository;
        private readonly IFicheroEntidadService _ficheroEntidadService;

        public EntidadesController(IEntidadesRepository entidadesRepository, IFicheroEntidadService ficheroEntidadService)
        {
            _entidadesRepository = entidadesRepository;
            _ficheroEntidadService = ficheroEntidadService;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Cargar()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Cargar(IFormFile file)
        {


            try
            {
                ConjuntoEntidad conjuntoEntidad = _ficheroEntidadService.Cargar(file);
                //_muestraRepository.AltaConjuntoMuestra(conjuntoMuestra);
                _entidadesRepository.AltaConjuntoEntidad(conjuntoEntidad);
                Alert($"archivo cargado: {file.FileName} correctamente", NotificationType.success);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Alert($"Error al cargar el archivo, revise el formato", NotificationType.error);
            }


            return View();

        }


        public IActionResult GetAll()
        {
            return Json(new { Data = _entidadesRepository.GetAll() });
        }

        public IActionResult Ver(string id)
        {
            EntidadVM entidadVM = new EntidadVM()
            {
                Id = id,
                EntidadDetalleDto = _entidadesRepository.GetDetalle(id)
            };


            return View(entidadVM);
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            try
            {
                var objFromDeb = _entidadesRepository.GetById(id);
                if (objFromDeb == null)
                {
                    return Json(new { success = false, message = "Error borrando Entidad" });
                }
                _entidadesRepository.Delete(id);

                return Json(new { success = true, message = "Entidad borrada" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error borrando Entidad" });
            }
        }
    }
}