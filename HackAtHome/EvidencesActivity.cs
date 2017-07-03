using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using HackAtHome.Entities;
using HackAtHome.SAL;
using HackAtHome.CustomAdapters;
using HackAtHome.Fragments;

namespace HackAtHome
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/hackathome")]
    public class EvidencesActivity : Activity
    {
        public const string EVIDENCE_TITLE = "EVIDENCE_TITLE";
        public const string EVIDENCE_STATUS = "EVIDENCE_STATUS";
        public const string EVIDENCE_DESCRIPTION = "EVIDENCE_DESCRIPTION";
        public const string EVIDENCE_IMAGEURL = "EVIDENCE_IMAGEURL";

        private TextView UserNameTextView;
        private ListView EvidencesListView;

        private EvidencesFragment Data;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Evidences);

            UserNameTextView = FindViewById<TextView>(Resource.Id.UserNameTextView);
            EvidencesListView = FindViewById<ListView>(Resource.Id.EvidencesListView);

            EvidencesListView.ItemClick += EvidencesListView_ItemClick;

            LoadData();

            
            

        }

        private async void LoadData()
        {
            Data = (EvidencesFragment)this.FragmentManager.FindFragmentByTag("Data");
            if(Data == null)
            {
                Data = new EvidencesFragment();
                var fragmentTransaction = this.FragmentManager.BeginTransaction();
                fragmentTransaction.Add(Data, "Data");
                fragmentTransaction.Commit();
                await LoadNewData();
            }

            UserNameTextView.Text = Data.UserName;
            LoadListView();
        }

        private async Task LoadNewData()
        {
            var serviceClient = new ServiceClient();
            Data.Token = Intent.GetStringExtra(MainActivity.TOKEN);
            try
            {
               Data. Evidences = await serviceClient.GetEvidencesAsync(Data.Token);
            }
            catch (Exception)
            {
                ShowError();
                return;
            }

            Data.UserName = Intent.GetStringExtra(MainActivity.USERNAME);
        }

        private void LoadListView()
        {
            int listLayout, text1, text2;
            var orientation = this.WindowManager.DefaultDisplay.Rotation;


 
 
            if (orientation == SurfaceOrientation.Rotation90 || orientation == SurfaceOrientation.Rotation270)
            {
                listLayout = Resource.Layout.SimpleListItem;
                text1 = Resource.Id.TitleTextView;
                text2 = Resource.Id.StatusTextView;
            }
            else
            {
                listLayout = Android.Resource.Layout.SimpleListItem2;
                text1 = Android.Resource.Id.Text1;
                text2 = Android.Resource.Id.Text2;
            }




            //var adapter = new EvidencesAdapter(this, Data.Evidences, Android.Resource.Layout.SimpleListItem2,
            //  Android.Resource.Id.Text1, Android.Resource.Id.Text2);

            var adapter = new EvidencesAdapter(this, Data.Evidences, listLayout, text1, text2);

            EvidencesListView.Adapter = adapter;

            if(Data.EvidenceListState != null)
            {
                EvidencesListView.OnRestoreInstanceState(Data.EvidenceListState);
            }
        }

        private async void EvidencesListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(EvidenceDetailActivity));
            var evidenceItem = Data.Evidences[e.Position];
            var serviceClient = new ServiceClient();
            var evidenceDetail = await serviceClient.GetEvidenceByIDAsync(Data.Token, evidenceItem.EvidenceID);
            intent.PutExtra(EVIDENCE_TITLE, evidenceItem.Title);
            intent.PutExtra(EVIDENCE_STATUS, evidenceItem.Status);
            intent.PutExtra(MainActivity.USERNAME, UserNameTextView.Text);
            var evidenceDescription = @"<html>
                                        <head><title>Descripción</title>
                                        <style type='text/css'> body { color: white } </style>
                                        </head>
                                        <body>" + evidenceDetail.Description + "</body></html>";
                                            
            intent.PutExtra(EVIDENCE_DESCRIPTION, evidenceDescription);
            intent.PutExtra(EVIDENCE_IMAGEURL, evidenceDetail.Url);
            
            StartActivity(intent);
        }

        protected override void OnPause()
        {
            Data.EvidenceListState = EvidencesListView.OnSaveInstanceState();
            base.OnPause();
        }

        public void ShowError()
        {
            Toast.MakeText(this, "Ocurrió un error", ToastLength.Short).Show();
        }
    }
}