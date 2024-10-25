﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Sexshop_TutsiPop.Data;
using Sexshop_TutsiPop.Models;

namespace Sexshop_TutsiPop.Controllers
{
    public class empleadosController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public empleadosController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        // Acción para exportar datos a Excel
        public IActionResult ExportToExcel()
        {
            // Establecer el contexto de la licencia (NonCommercial License)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Obtener los datos de la base de datos
            var Empleados = _context.empleados.ToList();

            // Crear archivo Excel
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Empleados");

                // Definir las cabeceras
                worksheet.Cells[1, 1].Value = "Cedula Empleado";
                worksheet.Cells[1, 2].Value = "Nombre";
                worksheet.Cells[1, 3].Value = "Apellido";
                worksheet.Cells[1, 4].Value = "Correo";
                worksheet.Cells[1, 5].Value = "Fecha Contratacion";
                worksheet.Cells[1, 6].Value = "Rol";
                worksheet.Cells[1, 7].Value = "Salario";


                // Establecer formato de cabeceras (opcional)
                using (var range = worksheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightPink);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Agregar datos a las filas
                int row = 2;
                foreach (var empleados in Empleados)
                {
                    worksheet.Cells[row, 1].Value = empleados.cedula_empleado;
                    worksheet.Cells[row, 2].Value = empleados.nombre;
                    worksheet.Cells[row, 3].Value = empleados.apellido;
                    worksheet.Cells[row, 4].Value = empleados.correo;
                    worksheet.Cells[row, 5].Value = empleados.fecha_contratacion;
                    worksheet.Cells[row, 6].Value = empleados.rol;
                    worksheet.Cells[row, 7].Value = empleados.salario;


                    row++;
                }

                // Ajustar ancho de las columnas
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Retornar el archivo Excel como un archivo descargable
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string excelName = $"Empleados-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        // GET: empleados
        public async Task<IActionResult> Index()
        {
            return View(await _context.empleados.ToListAsync());
        }

        // GET: empleados/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleados = await _context.empleados
                .FirstOrDefaultAsync(m => m.cedula_empleado == id);
            if (empleados == null)
            {
                return NotFound();
            }

            return View(empleados);
        }

        // GET: empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("cedula_empleado,nombre,apellido,correo,numero_telefono,fecha_contratacion,rol,salario,activo")] empleados empleados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleados);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empleados);
        }

        // GET: empleados/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleados = await _context.empleados.FindAsync(id);
            if (empleados == null)
            {
                return NotFound();
            }
            return View(empleados);
        }

        // POST: empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("cedula_empleado,nombre,apellido,correo,numero_telefono,fecha_contratacion,rol,salario,activo")] empleados empleados)
        {
            if (id != empleados.cedula_empleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleados);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!empleadosExists(empleados.cedula_empleado))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(empleados);
        }

        // GET: empleados/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleados = await _context.empleados
                .FirstOrDefaultAsync(m => m.cedula_empleado == id);
            if (empleados == null)
            {
                return NotFound();
            }

            return View(empleados);
        }

        // POST: empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var empleados = await _context.empleados.FindAsync(id);
            if (empleados != null)
            {
                _context.empleados.Remove(empleados);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool empleadosExists(string id)
        {
            return _context.empleados.Any(e => e.cedula_empleado == id);
        }
    }
}