using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;
using ToDoApplication.Models;
using System.Data;
using System.Collections.Generic;
using System.Configuration;

namespace DotNetAppSqlDb.Controllers
{
    public class TodosController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);

        // GET: Todos
        public ActionResult Index()
        {
                con.Open();
                SqlCommand com = new SqlCommand(); 
                com.Connection = con; 
                com.CommandType = CommandType.Text; 
                com.CommandText = "select * from Todoes";
                SqlDataReader reader;
                IList<Todo> data = new List<Todo>();

                reader= com.ExecuteReader();
                while (reader.Read())
                {
                    Todo todoObj = new Todo();
                    todoObj.ID = (int)reader["ID"];
                    todoObj.Description = (string)reader["DESCRIPTION"];
                    todoObj.CreatedDate = (DateTime)reader["CREATEDDATE"];
                    data.Add(todoObj);
                }
                
                return View(data);
           
        }

        // GET: Todos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todoObj;
               con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                com.CommandText = "select * from Todoes where ID="+id.ToString();
                SqlDataReader reader;
                reader = com.ExecuteReader();
                todoObj = new Todo();
                while (reader.Read())
                {                                    
                    todoObj.ID = (int)reader["ID"];
                    todoObj.Description = (string)reader["DESCRIPTION"];
                    todoObj.CreatedDate = (DateTime)reader["CREATEDDATE"];
                }
                if (todoObj == null)
                {
                    return HttpNotFound();
                }
                return View(todoObj);
            
        }

        // GET: Todos/Create
        public ActionResult Create()
        {
            return View(new Todo { CreatedDate = DateTime.Now });
        }

        // POST: Todos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Description,CreatedDate")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                string cmdString = "INSERT INTO Todoes(Description,CreatedDate) VALUES (@val1, @val2)";
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;
                    com.CommandText = cmdString;
                    com.Parameters.AddWithValue("@val1", todo.Description);
                    com.Parameters.AddWithValue("@val2", todo.CreatedDate);
            
                    com.ExecuteNonQuery();
                    return RedirectToAction("Index");
            }
            return View(todo);
        }

        // GET: Todos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                com.CommandText = "select * from Todoes where ID=" + id.ToString();
                SqlDataReader reader;
                IList<Todo> data = new List<Todo>();
                Todo todoObj = new Todo();
                reader = com.ExecuteReader();
                while (reader.Read())
                {
                    todoObj.ID = (int)reader["ID"];
                    todoObj.Description = (string)reader["DESCRIPTION"];
                    todoObj.CreatedDate = (DateTime)reader["CREATEDDATE"];
                }
                return View(todoObj);
        }

        // POST: Todos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Description,CreatedDate")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;
                    com.CommandText = "UPDATE Todoes SET Description = @val1,CreatedDate = @val2 where Id=@val3";
                    com.Parameters.AddWithValue("@val1", todo.Description);
                    com.Parameters.AddWithValue("@val2", todo.CreatedDate);
                    com.Parameters.AddWithValue("@val3", todo.ID);
                    com.ExecuteNonQuery();
                    return RedirectToAction("Index");
            }
            return View(todo);
        }

        // GET: Todos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todoObj;
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                com.CommandText = "select * from Todoes where ID=" + id.ToString();
                SqlDataReader reader;
                reader = com.ExecuteReader();
                todoObj = new Todo();
                while (reader.Read())
                {
                    todoObj.ID = (int)reader["ID"];
                    todoObj.Description = (string)reader["DESCRIPTION"];
                    todoObj.CreatedDate = (DateTime)reader["CREATEDDATE"];
                }
                if (todoObj == null)
                {
                    return HttpNotFound();
                }
                return View(todoObj);
        }

        // POST: Todos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Todo todoObj;
             con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                com.CommandText = "delete from Todoes where ID=" + id.ToString();
                SqlDataReader reader;
                reader = com.ExecuteReader();
                todoObj = new Todo();
                while (reader.Read())
                {
                    todoObj.ID = (int)reader["ID"];
                    todoObj.Description = (string)reader["DESCRIPTION"];
                    todoObj.CreatedDate = (DateTime)reader["CREATEDDATE"];
                }
                if (todoObj == null)
                {
                    return HttpNotFound();
                }
                return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                con.Close();
            }
            base.Dispose(disposing);
        }
    }
}