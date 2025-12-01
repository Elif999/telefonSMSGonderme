using System.Text.RegularExpressions;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System;
using System.Windows.Forms;

namespace WinFormsApp70
{
    public partial class Form1 : Form
    {
        string accountSid = "*****************************"; // Twilio'dan alınan
        string authToken = "*****************************";   // Twilio'dan alınan
        string twilioNumber = "*********";    // Twilio'dan aldığın numara
        public Form1()
        {
            InitializeComponent();
           

        }
        private void KontrolButonlar()
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text) &&
                !string.IsNullOrWhiteSpace(textBox2.Text))

            {
                button1.Enabled = true; // Oluştur butonu
                button2.Enabled = true; // Giriş butonu

            }
            else
            {
                button1.Enabled = false; // Oluştur butonu
                button2.Enabled = false; // Giriş butonu

            }
        }
        class Hesap
        {
            public string isim, sifre, tel;
            public string giris(string i, string s, string t)
            {
                MessageBox.Show("isim: " + i + " -Şifre: " + s + "sifreniz başarıyla oluşturuldu", "Bilgi");
                return i;
            }
        }
        Hesap h = new Hesap();
        private void Form1_Load(object sender, EventArgs e)
        {
           

            label3.Visible = false;
            label4.Visible = false;
            label6.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("İsim alanı boş olamaz. Lütfen bir isim girin.", "Hata");
                return;
            }

            // Şifre kontrolü
            if (string.IsNullOrWhiteSpace(textBox2.Text) || textBox2.Text.Length < 6)
            {
                MessageBox.Show("Şifre en az 6 karakter olmalıdır.", "Hata");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox4.Text) || !Regex.IsMatch(textBox4.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Lütfen geçerli bir telefon numarası girin (10 haneli).", "Hata");
                button1.Enabled = false;
                return;
            }

            button1.Visible = false;
            h.isim = textBox1.Text.ToString();
            h.sifre = textBox2.Text.ToString();
            h.tel = textBox4.Text.ToString();
            h.giris(h.isim, h.sifre, h.tel);
            label4.Text = h.isim.ToString();
            label3.Text = h.sifre.ToString();
            label6.Text = h.tel.ToString();
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            label7.Visible = false;
            textBox4.Visible = false;
            button2.Visible = true;
            button4.Visible = true;
            button4.Text = "şifremi unuttum";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (h.isim == textBox1.Text && h.sifre == textBox2.Text)
            {
                MessageBox.Show("Giriş başarılı", "Bilgi");
                Form2 fr2 = new Form2();
                fr2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Sifrenizi kontrol ediniz veya bir şifreniz yoksa önce şifre oluşturun", "Giriş başarısız");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            TwilioClient.Init(accountSid, authToken);

            button2.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            label7.Visible = true;
            textBox4.Visible = true;
            button1.Visible = false;

            button4.Text = "Yenile";
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Lütfen telefon numarasını girin.", "Hata");
                return; // Fonksiyondan çık
            }
            var phoneNumber = textBox4.Text.Trim();
            if (phoneNumber.StartsWith("0"))
            {
                phoneNumber = phoneNumber.Substring(1); // Başındaki 0'ı çıkart
            }

            var toPhone = "+90" + phoneNumber; // Türkiye numarasına +90 ekle

            if (textBox4.Text == h.tel)
            {

                MessageBox.Show("şifrenizi yenileyebilirsiniz");
                label1.Visible = true;
                label2.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                button1.Visible = true;
                label7.Visible = true;
                textBox4.Visible = true;
                button4.Visible = false;

                label3.Text = " ";
                label4.Text = " ";
                label6.Text = " ";

                var from = new PhoneNumber(twilioNumber); // Twilio numaran
                var to = new PhoneNumber(toPhone); // Alıcı numarası
                var body = $"Merhaba! Şu anda sistemdeki aktiviteyi kontrol ediyorum, kısa süre içinde geri dönüş yapacağım.";

                try
                {
                    // SMS gönderme
                    var message = MessageResource.Create(
                        to: to,
                        from: from,
                        body: body
                      
                    );

                    MessageBox.Show($"Merhaba, şifrenizi sıfırlamak için bu mesajı aldınız.şifrenizi yenileyebilirsiniz"); // Teslimat durumu için callback URL" +
                
                }
                catch (Exception ex)
                {
                    // Hata durumunda kullanıcıya detaylı bilgi ver
                    MessageBox.Show($"Hata oluştu: {ex.Message}\nStack Trace: {ex.StackTrace}", "Hata");
                }
            }
            else
            {
                MessageBox.Show("Girilen telefon numarası ile hesabınız eşleşmiyor", "Hata");
            }
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            KontrolButonlar();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            KontrolButonlar();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            KontrolButonlar();
        }
    }
}
