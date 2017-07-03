using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using HackAtHome.SAL;
using HackAtHome.Entities;

namespace HackAtHome
{

    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/hackathome")]
    public class MainActivity : Activity
    {
        public const string USERNAME = "USERNAME";
        public const string TOKEN = "TOKEN";

        private Button ValidateButton;
        private EditText EmailText;
        private EditText PasswordText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            ValidateButton = FindViewById<Button>(Resource.Id.ValidateButton);

            ValidateButton.Click += ValidateButton_Click;

            EmailText = FindViewById<EditText>(Resource.Id.EmailText);
            PasswordText = FindViewById<EditText>(Resource.Id.PasswordText);
        }

        private async void ValidateButton_Click(object sender, System.EventArgs e)
        {
            var serviceClient = new ServiceClient();
            var studentEmail = EmailText.Text;
            var studentPassword = PasswordText.Text;

            var result = await serviceClient.AutenticateAsync(studentEmail, studentPassword);
            var microsoftEvidence = new LabItem
            {
                Email = EmailText.Text,
                Lab = "Hack@Home",
                DeviceId = Android.Provider.Settings.Secure
                .GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId)
            };

            var microsoftServiceClient = new MicrosoftServiceClient();
            await microsoftServiceClient.SendEvidence(microsoftEvidence);

            if(result.Status == Entities.Status.AllSuccess || result.Status == Entities.Status.Success)
            {
                var intent = new Intent(this, typeof(EvidencesActivity));
                intent.PutExtra(USERNAME, result.FullName);
                intent.PutExtra(TOKEN, result.Token);

                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Error al autenticar", ToastLength.Short).Show();
            }
            
        }
    }
}

