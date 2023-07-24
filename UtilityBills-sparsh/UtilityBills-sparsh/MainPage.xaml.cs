using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;

namespace UtilityBills_sparsh {
   public partial class MainPage : ContentPage {

      public string province;
      public MainPage() {
         InitializeComponent();
      }

      private void btnReset_Clicked(object sender, EventArgs e) {
         xProvience.SelectedItem = null;

         xUsage.Text = string.Empty;
         xEveningUsage.Text = string.Empty;

         lblResult.IsVisible = false;
         lblResult5.IsVisible = false;
         xError.IsVisible = false;

         lblResult0.Text = $"";
         lblResult1.Text = $"";
         lblResult2.Text = $"";
         lblResult3.Text = $"";
         lblResult4.Text = $"";
         lblResult5.Text = $"";
        
      }
      private void btnCalculate_Clicked(object sender, EventArgs e) {
         Console.WriteLine("You have clicked the calculate button");

         // Get Daytime Usage
         string DayUsage = xUsage.Text;

         // Get Evening Usage
         string EveUsage = xEveningUsage.Text;
        
         double DayCharge;
         double EveCharge;
         double TotalUsage;
         double SaleTax = 0.0;
         double EnvRebate = 0.0;
         double Total;

         if (ValidateFields()) {
            xError.IsVisible = false;

            if (Int32.TryParse(DayUsage, out int dayuse)) {

               if (double.TryParse(EveUsage, out double eveuse)) {
                  DayCharge = dayuse * 0.314;
                  EveCharge = eveuse * 0.186;
                  TotalUsage = DayCharge + EveCharge;

                  if (province == "ON") {
                     SaleTax = 0.13 * TotalUsage;
                     lblResult3.Text = $"Sales Tax (13%): ${SaleTax.ToString("0.00")}";
                  }
                  else if (province == "BC") {
                     SaleTax = 0.12 * TotalUsage;
                     EnvRebate = 0.095 * TotalUsage;
                     lblResult3.Text = $"Sales Tax (12%): ${SaleTax.ToString("0.00")}";
                  }
                  else if (province == "AB") {
                     SaleTax = 0.0;
                     lblResult3.Text = $"Sales Tax (0%): ${SaleTax.ToString("0.00")}";
                  }
                  else if (province == "NL") {
                     SaleTax = 0.15 * TotalUsage;
                     lblResult3.Text = $"Sales Tax (15%): ${SaleTax.ToString("0.00")}";
                  }

                  if (switchStatusLabel.Text == "ON" && province != "BC") {
                     EnvRebate = 0.095 * TotalUsage;
                  }
                  Total = SaleTax + TotalUsage - EnvRebate;

                  lblResult.IsVisible = true;
                  lblResult0.Text = $"Daytime usage charge: ${DayCharge.ToString("0.00")}";
                  lblResult1.Text = $"Evening usage charge: ${EveCharge.ToString("0.00")}";
                  lblResult2.Text = $"Total usage charge: ${TotalUsage.ToString("0.00")}";
                  lblResult4.Text = $"Environmental rebate: -${EnvRebate.ToString("0.00")}";
                  lblResult5.Text = $"You Must Pay: ${Total.ToString("0.00")}";
                  lblResult5.IsVisible = true;

               }
               else {
                  xError.IsVisible = true;
               }

            }
            else {
               xError.IsVisible = true;
            }
         }
         else {
            xError.IsVisible = true;
         }
      }

      private void switchState_Toggled(object sender, ToggledEventArgs e) {

         if (e.Value)
            switchStatusLabel.Text = "ON";
         else
            switchStatusLabel.Text = "OFF";
      }
      private void provincePicker_SelectedIndexChanged(object sender, EventArgs e) {
         province = "";
         province = xProvience.SelectedItem as string;
         bool isBCProvince = province == "BC";

         switchState.IsToggled = isBCProvince;
         switchState.IsEnabled = !isBCProvince;
      }

      private bool ValidateFields() {
         bool isValid = true;

         if (string.IsNullOrWhiteSpace(xProvience.SelectedItem as string))
            isValid = false;

         if (string.IsNullOrWhiteSpace(xUsage.Text))
            isValid = false;

         if (string.IsNullOrWhiteSpace(xEveningUsage.Text))
            isValid = false;

         return isValid;

      }
   }
}
