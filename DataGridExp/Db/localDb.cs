using DataGridExp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGridExp.Db
{
    public class localDb
    {
        public localDb()
        {
            Init();
        }

        public List<Student> Studentls;

        public void Init()
        {
            Studentls = new List<Student>();
            for (int i = 0; i < 30; i++)
            {
                Studentls.Add(new Student()
                {
                    Id = i,
                    Name = $"Sample{i}"
                });
            }
        }

        public List<Student> GetStudents()
        {
            return Studentls;
        }

        //增
        public void AddStudent(Student stu)
        {
            Studentls.Add(stu);
        }

        //删
        public void DelStudent(int id)
        {
            var model = Studentls.FirstOrDefault(t => t.Id == id);
            if (model != null)
            {
                Studentls.Remove(model);
            }
        }

        //查
        public List<Student> GetStudentsByName(string name)
        {
            return Studentls.Where(q => q.Name.Contains(name)).ToList();
        }

        public Student GetStudentById(int id)
        {
            var model = Studentls.FirstOrDefault(t => t.Id == id);

            if (model != null)
            {
                return new Student()
                {
                    Id = model.Id,
                    Name = model.Name
                };
            }
            return null;
        }
    }
}