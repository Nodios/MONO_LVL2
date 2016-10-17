﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Service;
using System.Net;

namespace Project.MVC.Controllers
{
    public class ModelController : Controller
    {
        private VehicleService vehicleService = VehicleService.GetInstance();
        // GET: Model
        public ActionResult Index(string search, int? page, string sortBy)
        {
            ViewBag.SortByIdParameter = string.IsNullOrEmpty(sortBy) ? "Id desc" : "";
            ViewBag.SortByNameParameter = sortBy == "Name" ? "Name desc" : "Name";
            ViewBag.SortByMakerNameParameter = sortBy == "MakerName" ? "MakerName desc" : "MakerName";


            return View( vehicleService.GetVehicleModels(search, page, sortBy) );
        }


        public ActionResult Add()
        {
            ViewBag.VehicleMakersList = vehicleService.GetVehicleMakesAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "VehicleModelId,VehicleMakeId,Name,Abrv")] VehicleModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                if( vehicleService.Add(vehicleModel) )
                    return RedirectToAction("Index");
                else
                {
                    ViewBag.Error = "Couldn't add model. There is no such maker.";
                    return View("Index", vehicleService.GetVehicleModels("", 1, "") );
                }
            }
            return View(vehicleModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            VehicleModel vehicleModel = vehicleService.FindByIdModel(id);

            if (vehicleModel == null)
                return HttpNotFound();

            ViewBag.VehicleMakersList = vehicleService.GetVehicleMakesAll();

            return View(vehicleModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VehicleModelId,VehicleMakeId,Name,Abrv")] VehicleModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                vehicleService.Edit(vehicleModel);
                return RedirectToAction("Index");
            }

            return View(vehicleModel);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            VehicleModel vehicleModel = vehicleService.FindByIdModel(id);

            if (vehicleModel == null)
                return HttpNotFound();

            return View(vehicleModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(vehicleService.FindByIdModel(id));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteModelConfirmed(int id)
        {
            vehicleService.RemoveModel(id);
            return RedirectToAction("Index");
        }

    }
}