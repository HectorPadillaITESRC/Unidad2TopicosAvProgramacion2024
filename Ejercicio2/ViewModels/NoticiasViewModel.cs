using CommunityToolkit.Mvvm.Input;
using Ejercicio2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ejercicio2.ViewModels
{

    public enum Vistas { Lista, Agregar, Editar, Eliminar, Detalles }

    public class NoticiasViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Noticias> Noticias { get; set; } = new();
        public Noticias? Noticia { get; set; }

        public ICommand AgregarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }

        public Vistas Vista { get; set; } = Vistas.Lista;
        public string Error { get; set; } = "";


        public NoticiasViewModel()
        {
            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(Editar);
            EliminarCommand = new RelayCommand(Eliminar);
            CambiarVistaCommand = new RelayCommand<Vistas>(CambiarVista);
            Deserializar();
        }

        int posNoticiaEditar;

        private void CambiarVista(Vistas vistas)
        {

            if (vistas == Vistas.Agregar)
            {
                Noticia = new()
                {
                    Fecha = DateTime.Now.Date
                };

            }

            if (vistas == Vistas.Editar && Noticia != null)
            {
                var clon = new Noticias
                {
                    Contenido = Noticia.Contenido,
                    Fecha = Noticia.Fecha,
                    Titulo = Noticia.Titulo,
                    URLImagen = Noticia.URLImagen
                };

                posNoticiaEditar = Noticias.IndexOf(Noticia);

                Noticia = clon;
            }

            Error = "";
            Actualizar(nameof(Error));

            Vista = vistas;
            Actualizar(nameof(Vista));

            Actualizar(nameof(Noticia));

        }

        private void Eliminar()
        {
            if (Noticia != null)
            {
                Noticias.Remove(Noticia);
                Serializar();
                CambiarVista(Vistas.Lista);
            }
        }

        private void Editar()
        {
            if (Noticia != null)
            {
                Error = "";
                //Validar

                if (string.IsNullOrWhiteSpace(Noticia.Titulo))
                {
                    Error += "- Escriba el titulo de la noticia.\n";
                }
                if (string.IsNullOrWhiteSpace(Noticia.Contenido))
                {
                    Error += "- Escriba el contenido de la noticia.\n";
                }
                if (string.IsNullOrWhiteSpace(Noticia.URLImagen))
                {
                    Error += "- Escriba la URL de la fotografía de la noticia.\n";
                }
                else if (!Noticia.URLImagen.StartsWith("http") || !Noticia.URLImagen.EndsWith(".jpg"))
                {
                    Error += "- Escriba la URL de la fotografía en formato JPG.\n";
                }
                if (Noticia.Fecha.Date > DateTime.Now.Date)
                {
                    Error += "- No se deben redactar noticias a futuro.\n";
                }
                Actualizar(nameof(Error));

                if (Error == "") //Paso todas las validaciones
                {

                    Noticias[posNoticiaEditar] = Noticia;

                    Serializar();
                    CambiarVista(Vistas.Lista);
                }

            }
        }

        private void Agregar()
        {
            if (Noticia != null)
            {
                Error = "";
                //Validar

                if (string.IsNullOrWhiteSpace(Noticia.Titulo))
                {
                    Error += "- Escriba el titulo de la noticia.\n";
                }
                if (string.IsNullOrWhiteSpace(Noticia.Contenido))
                {
                    Error += "- Escriba el contenido de la noticia.\n";
                }
                if (string.IsNullOrWhiteSpace(Noticia.URLImagen))
                {
                    Error += "- Escriba la URL de la fotografía de la noticia.\n";
                }
                else if (!Noticia.URLImagen.StartsWith("http") || !Noticia.URLImagen.EndsWith(".jpg"))
                {
                    Error += "- Escriba la URL de la fotografía en formato JPG.\n";
                }
                if (Noticia.Fecha.Date > DateTime.Now.Date)
                {
                    Error += "- No se deben redactar noticias a futuro.\n";
                }
                Actualizar(nameof(Error));

                if (Error == "") //Paso todas las validaciones
                {
                    Noticias.Add(Noticia);
                    Serializar();
                    CambiarVista(Vistas.Lista);
                }

            }

        }

        private void Serializar()
        {
            File.WriteAllText("noticias.json",
            JsonSerializer.Serialize(Noticias));
        }

        private void Deserializar()
        {
            if (File.Exists("noticias.json"))
            {
                var datos = JsonSerializer
                    .Deserialize<ObservableCollection<Noticias>>(File.ReadAllText("noticias.json"));

                if (datos != null)
                {
                    foreach (var n in datos)
                    {
                        Noticias.Add(n);
                    }
                }
            }
        }

        void Actualizar(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
