using Lab13.Models;
using Lab13.Sevices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lab13.ViewModels
{
    public class AlbumViewModel : BaseViewModel
    {
        #region Services
        private readonly ArtistaService dataServiceArtistas;
        private readonly AlbumService dataServiceAlbumes;
        #endregion

        #region Attributes
        private ObservableCollection<Artista> artistas;
        private Artista selectedArtista;
        private string titulo;
        private double precio;
        private int anio;
        #endregion

        #region Properties
        public ObservableCollection<Artista> Artistas
        {
            get { return this.artistas; }
            set { SetValue(ref this.artistas, value); }
        }

        public Artista SelectedArtista
        {
            get { return this.selectedArtista; }
            set { SetValue(ref this.selectedArtista, value); }
        }

        public string Titulo
        {
            get { return this.titulo; }
            set { SetValue(ref this.titulo, value); }
        }

        public double Precio
        {
            get { return this.precio; }
            set { SetValue(ref this.precio, value); }
        }

        public int Anio
        {
            get { return this.anio; }
            set { SetValue(ref this.anio, value); }
        }


        #endregion

        #region Commands
        public ICommand CreateCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var newAlbum = new Album()
                    {
                        ArtistaID = this.SelectedArtista.ArtistaID,
                        Titulo = this.Titulo,
                        Precio = this.Precio,
                        Anio = this.Anio
                    };

                    var album = this.dataServiceAlbumes.GetByTitulo(newAlbum.Titulo);

                    if (album == null)
                    {
                        if (newAlbum != null)
                        {
                            if (this.dataServiceAlbumes.Create(newAlbum))
                            {
                                await Application.Current.MainPage.DisplayAlert("Operación Exitosa",
                                                                                $"Albúm del artista: {this.SelectedArtista.Nombre} " +
                                                                                $"creado correctamente en la base de datos",
                                                                                "Ok");

                                this.SelectedArtista = null;
                                this.Titulo = string.Empty;
                                this.Precio = 0;
                                this.Anio = DateTime.Now.Year;
                                await Application.Current.MainPage.Navigation.PopAsync();

                            }

                            else
                                await Application.Current.MainPage.DisplayAlert("Operación Fallida",
                                                                                $"Error al crear el Albúm en la base de datos",
                                                                                "Ok");
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Validación",
                                                                              $"Título Repetido",
                                                                              "Ok");
                    }


                });
            }
        }
        #endregion Commands

        #region Constructor
        public AlbumViewModel()
        {
            this.dataServiceArtistas = new ArtistaService();
            this.dataServiceAlbumes = new AlbumService();
            this.LoadArtistas();
            this.Anio = DateTime.Now.Year;
        }
        #endregion Constructor






        #region Methods
        private void LoadArtistas()
        {
            var artistasDB = this.dataServiceArtistas.Get();
            this.Artistas = new ObservableCollection<Artista>(artistasDB);
        }
        #endregion
    }

}