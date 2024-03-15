using Ejercicio1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json;
using System.IO;

namespace Ejercicio1.ViewModels
{

    public enum Ventanas { Agregar, Editar, Eliminar, Lista }

    public class LibrosViewModel : INotifyPropertyChanged
    {
        //Coleccion de elementos del modelo
        public ObservableCollection<Libro> Libros { get; set; } = new();
        //Acciones que realiza el usuario
        public ICommand AgregarCommand { get; set; }
        public ICommand VerAgregarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ICommand VerEliminarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        //Referencia a un Modelo
        public Libro? Libro { get; set; }
        //PAra mostrar errores en las vistas
        public string Error { get; set; } = "";

        //Se utiliza para decidir que ventana mostrar
        public Ventanas Ventana { get; set; } = Ventanas.Lista;

        public LibrosViewModel()
        {
            CancelarCommand = new RelayCommand(Cancelar);
            AgregarCommand = new RelayCommand(Agregar);
            GuardarCommand = new RelayCommand(Guardar);
            VerAgregarCommand = new RelayCommand(VerAgregar);
            VerEditarCommand = new RelayCommand(VerEditar);
            VerEliminarCommand = new RelayCommand(VerEliminar);
            EliminarCommand = new RelayCommand(Eliminar);

            Deserializar();
        }

        private void Eliminar()
        {
            if (Libro != null)
            {
                Libros.Remove(Libro);
                Serializar();
                Ventana = Ventanas.Lista;
                Actualizar(nameof(Ventana));

            }
        }

        private void VerEliminar()
        {
            Ventana = Ventanas.Eliminar;
            Actualizar(nameof(Ventana));
        }

        int posAnterior;

        private void VerEditar()
        {
            if (Libro != null)
            {
                var clon = new Libro
                {
                    Autor = Libro.Autor,
                    ImagenPortada = Libro.ImagenPortada,
                    Titulo = Libro.Titulo
                };

                posAnterior = Libros.IndexOf(Libro); //posicion donde estaba el libro original
                Libro = clon;

                Ventana = Ventanas.Editar;
                Actualizar(nameof(Ventana));
            }
        }

        private void VerAgregar()
        {
            Libro = new();//Aqui se capturaran los datos del libro a añadir
            Error = "";

            Ventana = Ventanas.Agregar;
            Actualizar(nameof(Ventana));
            Actualizar(nameof(Error));

        }



        private void Guardar()
        {
            if (Libro != null)
            {
                Error = "";

                //Validar
                if (string.IsNullOrWhiteSpace(Libro.Titulo))
                {
                    Error = "Escriba el titulo del libro.";
                }
                if (string.IsNullOrWhiteSpace(Libro.Autor))
                {
                    Error = "Escriba el autor del libro";
                }
                if (string.IsNullOrWhiteSpace(Libro.ImagenPortada) || !Libro.ImagenPortada.StartsWith("http")
                   || !Libro.ImagenPortada.EndsWith(".jpg"))
                {
                    Error = "Escriba la dirección de una imagen en JPEG.";
                }

                Actualizar(nameof(Error));


                if (Error == "")//Si no hay error lo guardo
                {
                    Libros[posAnterior] = Libro; //Reemplazo en la posicion original del libro a editar
                    Serializar();

                    Ventana = Ventanas.Lista;
                    Actualizar(nameof(Ventana));
                }

            }
        }

        private void Agregar()
        {
            if (Libro != null)
            {
                Error = "";

                //Validar
                if (string.IsNullOrWhiteSpace(Libro.Titulo))
                {
                    Error = "Escriba el titulo del libro.";
                }
                if (string.IsNullOrWhiteSpace(Libro.Autor))
                {
                    Error = "Escriba el autor del libro";
                }
                if (string.IsNullOrWhiteSpace(Libro.ImagenPortada) || !Libro.ImagenPortada.StartsWith("http")
                   || !Libro.ImagenPortada.EndsWith(".jpg"))
                {
                    Error = "Escriba la dirección de una imagen en JPEG.";
                }

                if (!string.IsNullOrWhiteSpace(Error))
                {
                    Actualizar(nameof(Error));
                    return;
                }

                Libros.Add(Libro);

                //Serializar
                Serializar();

                Ventana = Ventanas.Lista;
                Actualizar(nameof(Ventana));

            }
        }

        void Serializar()
        {
            File.WriteAllText("libros.json", JsonSerializer.Serialize(Libros));
        }
        void Deserializar()
        {
            try
            {
                var datos = JsonSerializer.Deserialize<ObservableCollection<Libro>>(File.ReadAllText("libros.json"));
                if (datos != null)
                {
                    foreach (var libro in datos)
                    {
                        Libros.Add(libro);
                    }
                }
            }
            catch { }
        }



        private void Cancelar()
        {
            Ventana = Ventanas.Lista;
            Actualizar(nameof(Ventana));
        }



        private void Actualizar(string? name)
        {
            PropertyChanged?.Invoke(this, new(name));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
