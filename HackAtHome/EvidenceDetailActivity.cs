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
using Android.Webkit;

namespace HackAtHome
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/hackathome")]
    public class EvidenceDetailActivity : Activity
    {
        private TextView UserNameTextView;
        private TextView EvidenceTitleTextView;
        private TextView EvidenceStatusTextView;
        private WebView EvidenceDescriptionWebView;
        private ImageView EvidenceImageView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EvidenceDetail);

            UserNameTextView = FindViewById<TextView>(Resource.Id.UserNameTextView);
            EvidenceTitleTextView = FindViewById<TextView>(Resource.Id.EvidenceTitleTextView);
            EvidenceStatusTextView = FindViewById<TextView>(Resource.Id.EvidenceStatusTextView);
            EvidenceDescriptionWebView = FindViewById<WebView>(Resource.Id.EvidenceDescriptionWebView);
            EvidenceImageView = FindViewById<ImageView>(Resource.Id.EvidenceImageView);

            UserNameTextView.Text = Intent.GetStringExtra(MainActivity.USERNAME);
            EvidenceTitleTextView.Text = Intent.GetStringExtra(EvidencesActivity.EVIDENCE_TITLE);
            EvidenceStatusTextView.Text = Intent.GetStringExtra(EvidencesActivity.EVIDENCE_STATUS);
            EvidenceDescriptionWebView.LoadDataWithBaseURL(null, Intent.GetStringExtra(EvidencesActivity.EVIDENCE_DESCRIPTION), "text/html",
                "utf-8", null);
            EvidenceDescriptionWebView.SetBackgroundColor(Android.Graphics.Color.Transparent);

            var imageUrl = Intent.GetStringExtra(EvidencesActivity.EVIDENCE_IMAGEURL);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                Koush.UrlImageViewHelper.SetUrlDrawable(EvidenceImageView, imageUrl);
            }
        }
    }
}