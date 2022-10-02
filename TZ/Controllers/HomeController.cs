using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TZ.Models;

namespace TZ.Controllers
{
    public class HomeController : Controller
    {
        private unitsdbbContext unitsdbbContext;

        public HomeController()
        {
            unitsdbbContext = new unitsdbbContext();
        }

        // Проверяем есть ли у данного(нового) подразделения потомки.
        private ICollection<Unit> Check(string unit)
        {
            var units = unitsdbbContext.Units.ToArray();
            ICollection<Unit> result = new List<Unit>();
            foreach (var un in units)
            {
                if (un.Parentname == unit)
                {
                    result.Add(un);
                }
            }
            return result;
        }

        // Извлекаем родителельское подразделение
        private Unit? CheckParentUnit(string item)
        {
            var units = unitsdbbContext.Units.ToArray();
            if (item == null || item == "null")
                return null;
            return units.Where(x => x.Name == item).First();
        }

        private void sync(List<string[]> arr)
        {
            var units = unitsdbbContext.Units.ToArray();
            foreach (var item in arr)
            {
                // Имя подразделения в файле
                string name_unit = item[0];
                string parent_name = item[1];

                // Если нет подразделения в БД
                if (!(units.Any(x => x.Name == name_unit)))
                {
                    // То добавляем новое подразделение
                    unitsdbbContext.Add(new Unit { Name = name_unit, Status = true, Parentname = parent_name,
                    ParentnameNavigation = CheckParentUnit(parent_name),
                    InverseParentnameNavigation = Check(name_unit)});
                    unitsdbbContext.SaveChanges();
                }
                // Иначе сверяем их "родителей"
                else
                {
                    Unit? a = unitsdbbContext.Units.Where(x => x.Name == name_unit).First();
                    // Если родители не совпадают
                    if(a.Parentname is null || parent_name == "null" || parent_name is null) { }
                    else
                    {
                        a.Parentname = parent_name;
                        a.ParentnameNavigation = CheckParentUnit(parent_name);
                        unitsdbbContext.SaveChanges();
                    }
                }
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "units.txt");
            List<string[]> arr = new List<string[]>();

            using (StreamReader reader = new StreamReader(path))
            {
                string s;
                while ((s = reader.ReadLine()) != null)
                {
                    arr.Add(s.Split('|'));
                }
            }
            arr.RemoveAt(0);

            sync(arr);

            return View(unitsdbbContext.Units.ToArray());
        }

        [HttpPost]
        public IActionResult Index(string name)
        {
            var units = unitsdbbContext.Units.Where(x => x.Name.Contains(name) || x.Parentname.Contains(name)).ToArray();
            return View(units);
        }
    }
}