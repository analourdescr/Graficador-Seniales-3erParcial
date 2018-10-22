using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double amplitudMaxima = 1;

        public MainWindow()
        {
            InitializeComponent();

            /*plnGrafica.Points.Add(new Point(0, 10));
            plnGrafica.Points.Add(new Point(50, 20));
            plnGrafica.Points.Add(new Point(150, 10));
            plnGrafica.Points.Add(new Point(200, 50));
            plnGrafica.Points.Add(new Point(250, 0));
            plnGrafica.Points.Add(new Point(300, 100));
            plnGrafica.Points.Add(new Point(350, 30));
            plnGrafica.Points.Add(new Point(450, 50));
            plnGrafica.Points.Add(new Point(550, 100));
            plnGrafica.Points.Add(new Point(650, 10));
            plnGrafica.Points.Add(new Point(750, 25));
            plnGrafica.Points.Add(new Point(850, 120));
            plnGrafica.Points.Add(new Point(950, 30));
            plnGrafica.Points.Add(new Point(1050, 54));*/


            /*
            SeñalSenoidal señal = new SeñalSenoidal();

            for (double i = 0; i <= 1; i += 0.0001)
            {
                Console.WriteLine(señal.evaluar(i));
            }
            Console.ReadLine();
            */
        }

        private void BotonGraficar_Click(object sender, RoutedEventArgs e)
        {
            double tiempoInicial = double.Parse(txt_TiempoInicial.Text);
            double tiempoFinal = double.Parse(txt_TiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txt_FrecuenciaDeMuestreo.Text);

            Señal señal;
            Señal segundaSeñal;

            switch (cb_TipoSeñal.SelectedIndex)
            {
                // Señal Senoidal
                case 0:
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txt_Amplitud.Text);
                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txt_Fase.Text);
                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txt_Frecuencia.Text);

                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;

                // Señal Rampa
                case 1:
                    señal = new SeñalRampa();
                    break;

                // Señal Exponencial
                case 2:
                    double alpha = double.Parse(((ConfiguiracionSeñalExponencial)(panelConfiguracion.Children[0])).txt_Alpha.Text);
                    señal = new SeñalExponencial(alpha);
                    break;

                default:
                    señal = null;
                    break;
            }
            //segundaSeñal
            switch (cb_TipoSeñal2.SelectedIndex)
            {
                // Señal Senoidal
                case 0:
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion2.Children[0])).txt_Amplitud.Text);
                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion2.Children[0])).txt_Fase.Text);
                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion2.Children[0])).txt_Frecuencia.Text);

                    segundaSeñal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;

                // Señal Rampa
                case 1:
                    segundaSeñal = new SeñalRampa();
                    break;

                // Señal Exponencial
                case 2:
                    double alpha = double.Parse(((ConfiguiracionSeñalExponencial)(panelConfiguracion2.Children[0])).txt_Alpha.Text);
                    segundaSeñal = new SeñalExponencial(alpha);
                    break;

                default:
                    segundaSeñal = null;
                    break;
            }

            señal.TiempoInicial = tiempoInicial;
            señal.TiempoFinal = tiempoFinal;
            señal.FrecuenciaMuestreo = frecuenciaMuestreo;

            //segunda señal
            segundaSeñal.TiempoInicial = tiempoInicial;
            segundaSeñal.TiempoFinal = tiempoFinal;
            segundaSeñal.FrecuenciaMuestreo = frecuenciaMuestreo;

            señal.construirSeñalDigital();
            segundaSeñal.construirSeñalDigital();

            //Escalar
            double factorEscala = double.Parse(txtFactorEscalaAmplitud.Text);
            señal.escalar(factorEscala);

            // Desplazar
            if ((bool)ckb_Desplazamiento.IsChecked)
            {
                double factorDesplazamiento = double.Parse(txt_DesplazamientoEnY.Text);
                señal.desplazar(factorDesplazamiento);
            }

            //Truncar
            if ((bool)ckb_Truncar.IsChecked)
            {
                double n = double.Parse(txt_Truncar.Text);
                señal.truncar(n);
            }


            //Escalar2
            double factorEscala2 = double.Parse(txtFactorEscalaAmplitud2.Text);
            segundaSeñal.escalar(factorEscala2);

            // Desplazar2
            if ((bool)ckb_Desplazamiento2.IsChecked)
            {
                double factorDesplazamiento2 = double.Parse(txt_DesplazamientoEnY2.Text);
                segundaSeñal.desplazar(factorDesplazamiento2);
            }

            //Truncar2
            if ((bool)ckb_Truncar2.IsChecked)
            {
                double n = double.Parse(txt_Truncar2.Text);
                segundaSeñal.truncar(n);
            }

            // Actualizar
            señal.actualizarAmplitudMaxima();
            segundaSeñal.actualizarAmplitudMaxima();

            amplitudMaxima = señal.AmplitudMaxima;
            if (segundaSeñal.AmplitudMaxima > amplitudMaxima)
            {
                amplitudMaxima = segundaSeñal.AmplitudMaxima;
            }

            plnGrafica.Points.Clear();
            plnGraficaDos.Points.Clear();

            lbl_AmplitudMaxima.Text = amplitudMaxima.ToString("F");
            lbl_AmplitudMinima.Text = "-" + amplitudMaxima.ToString("F");

            if (señal != null)
            {
                // Sirve para recorrer una coleccion o arreglo
                foreach (Muestra muestra in señal.Muestras)
                {
                    plnGrafica.Points.Add(new Point((muestra.X - tiempoInicial) * scrContenedor.Width, (muestra.Y / amplitudMaxima * ((scrContenedor.Height / 2) - 30) * -1 + (scrContenedor.Height / 2))));
                }

            }

            //segunda senial
            if (segundaSeñal != null)
            {
                // Sirve para recorrer una coleccion o arreglo
                foreach (Muestra muestra in segundaSeñal.Muestras)
                {
                    plnGraficaDos.Points.Add(new Point((muestra.X - tiempoInicial) * scrContenedor.Width, (muestra.Y / amplitudMaxima * ((scrContenedor.Height / 2) - 30) * -1 + (scrContenedor.Height / 2))));
                }

                //lbl_AmplitudMaxima.Text = señal.AmplitudMaxima.ToString("F");
                //lbl_AmplitudMinima.Text = "-" + señal.AmplitudMaxima.ToString("F");
            }

            // Línea del Eje X
            plnEjeX.Points.Clear();
            plnEjeX.Points.Add(new Point(0, scrContenedor.Height / 2));
            plnEjeX.Points.Add(new Point((tiempoFinal - tiempoInicial) * scrContenedor.Width, scrContenedor.Height / 2));

            // Línea del Eje Y
            plnEjeY.Points.Clear();
            plnEjeY.Points.Add(new Point((-tiempoInicial) * scrContenedor.Width, 0));
            plnEjeY.Points.Add(new Point((-tiempoInicial) * scrContenedor.Width, scrContenedor.Height));
        }


        private void cb_TipoSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion.Children.Clear();
            switch (cb_TipoSeñal.SelectedIndex)
            {
                // Señal Senoidal
                case 0:
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;

                // Señal Rampa
                case 1:
                    break;
                default:
                    break;

                // Señal Senoidal
                case 2:
                    panelConfiguracion.Children.Add(new ConfiguiracionSeñalExponencial());
                    break;
            }
        }

        private void cb_TipoSeñal2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion2.Children.Clear();
            switch (cb_TipoSeñal2.SelectedIndex)
            {
                case 0://señal senoidal
                    panelConfiguracion2.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;
                case 1://Rampa

                    break;
                case 2://Exponencial
                    panelConfiguracion2.Children.Add(new ConfiguiracionSeñalExponencial());
                    break;
            }
        }
    }
}
