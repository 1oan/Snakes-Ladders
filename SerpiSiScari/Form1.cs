using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerpiSiScari
{
    public partial class Form1 : Form
    {
        private string numeJucator1; // numele primului jucator
        private Image pionJucator1; // pionul primului jucator
        private string numeJucator2; // numele celui de-al doilea jucator
        private Image pionJucator2; // pionul celui de-al doilea jucator
        private string[] NumePion = { "pionrosu", "pionportocaliu", "pionnegru", "pionbleu", "pionverde" };

        private PictureBox Jucator1; // imagine pion jucator1
        private PictureBox Jucator2; // imagine pion jucator2

        // pozitia si pozitia curenta a jucatorului 1,
        // ce ne vor ajuta in a muta jucatorul 1 pe tabla
        private int positionplayer1;
        private int currentPositionplayer1;

        // 3 Timere cu ajutorul carora putem face sa se vada
        // cum pionul se muta de pe o casuta pe alta
        private Timer MutaJucator1 = new Timer();
        private Timer MutaJucator2 = new Timer();
        private Timer MutaJucator = new Timer();

        // pozitia si pozitia curenta a jucatorului 2,
        // ce ne vor ajuta in a muta jucatorul 2 pe tabla
        private int positionplayer2;
        private int currentPositionplayer2;

        private List<PictureBox> Moves = new List<PictureBox>(); // lista de picturebox-uri pentru a construi tabla

        private bool TuraJucator = true; // aceste variabile vor determina a cui este randul sa dea cu zarul
        private bool TuraJucator1 = false;
        private bool TuraJucator2 = false;

        private Random rand = new Random(); // folosim pentru a da cu zaru

        private int i = 0; // casuta la care se afla primul jucator pe tabla
        private int j = 0; // casuta la care se afla jucatorul 2 pe tabla
        private int x;

        public Form1()
        {
            InitializeComponent();
        }

        private void SetupGamePVP()
        {
            this.BackgroundImage = Properties.Resources.fundalform;
            // In aceasta functie vom face tabla, cu cei 2 jucatori
            buttonZarPvsP.Visible = true; // butonul de aruncat cu zaru
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            int leftPos = 50; //leftpos o sa ne ajuta in a pozitiona casetele de la dreapta la stang 
            int topPos = 600; //toppos o sa ne ajuta sa punem casetele de jos in sus
            int a = 0; // a ne va ajuta sa punem 10 casete in linie


            for (int i = 0; i < 100; i++)
            {
                Image ImagineCaseta; // o variabila ce va atasa casetelor imagini pentru a forma tabla
                ImagineCaseta = Properties.Resources.ResourceManager.GetObject("_" + i.ToString()) as Image;

                // cream o caseta de tip picturebox de forma patrata
                PictureBox box = new PictureBox();
                box.BackgroundImage = ImagineCaseta;
                box.BackgroundImageLayout = ImageLayout.Stretch;
                box.Size = new Size(60, 60);

                // numim casetele
                box.Name = "box" + i.ToString(); // numele casetelor

                Moves.Add(box); //adaug casetele noi in lista de casete

                // dedesubt vom face algoritmul de care avem nevoie sa punem 10 casete in linie
                // punem casetele de la stanga la dreapta si apoi mutam in sus si inversam procesul
                // "a" controleaza cum pozitionam casetele 

                if (a == 10)
                {
                    // asta se intampla cand punem 10 casete de la stanga la dreapta
                    topPos -= 60; // in acest caz scadem 60 din "toppos" astfel incat sa punem urmatoarele caste pe un rand mai sus 
                    a = 30; // schimbam valoarea lui a la 30, facem asta pentru a pune casetele de la dreapta la stang acum
                }

                if (a == 20)
                {
                    // asta se intampla cand punem 10 casete de la dreapta la stanga
                    topPos -= 60; // din nou scadem 60 pentru a trece la un rand mai sus
                    a = 0; // setam a din nou la valorea 0
                }

                if (a > 20)
                {
                    // daca valoarea lui a este mai mare de 20
                    // punem casete de la dreapta la stanga
                    a--; //scadem 1 din a pentru fiecare caseta pe care o punem de la dreapta la stanga
                    box.Location = new Point(leftPos, topPos); // punem caseta pe pozitii
                    leftPos -= 60; //scadem 60 pentru a pune fiecare caseta de la dreapta la stanga
                }

                if (a < 10)
                {
                    // asta se intampla cand vrem sa punem casete de la stanga la dreapta
                    // daca a este mai mic decat 10

                    a++; //adaug 1 la a pentru fiecare caseta pe care o punem de la stanga la dreapta
                    leftPos += 60; //adaug 60 la leftpos pentru fiecare caseta pe care o punem de la stanga la dreapta
                    box.Location = new Point(leftPos, topPos); // punem caseta pe pozitii
                }
                this.Controls.Add(box); //in final adaugam caseta in interfata
            }

            MutaJucator1.Tick += MutaJucator1Event;
            MutaJucator1.Interval = 285;

            MutaJucator2.Tick += MutaJucator2Event;
            MutaJucator2.Interval = 285;

            //facem imaginile cu pionii jucatorilor si le asezam la pozitia de start
            
            //PION JUCATOR 1
            Jucator1 = new PictureBox();
            Jucator1.BackgroundImage = pionJucator1;
            Jucator1.BackgroundImageLayout = ImageLayout.Stretch;
            Jucator1.Size = new Size(50, 50);
            this.Controls.Add(Jucator1);
            Jucator1.BackColor = Color.Transparent;

            pictureBox1.BackgroundImage = pionJucator1;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.BackColor = Color.Transparent;
            
            //PION JUCATOR 2
            Jucator2 = new PictureBox();
            Jucator2.BackgroundImage = pionJucator2;
            Jucator2.BackgroundImageLayout = ImageLayout.Stretch;
            Jucator2.Size = new Size(50, 50);
            this.Controls.Add(Jucator2);
            Jucator2.BackColor = Color.Transparent;

            pictureBox2.BackgroundImage = pionJucator2;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.BackColor = Color.Transparent;

            MovePiece(Jucator1, "box0");
            MovePiece(Jucator2, "box0");
        }

        private void buttonZarPvsP_Click(object sender, EventArgs e)
        {
            // acest eveniment este atribuit butonului "Arunca zarul!",
            // astfel jucatorul va trebui sa apese pe buton pentru a 
            // face pionii sa se miste pe tabla

            //verificam daca este tura primului jucator
            if (TuraJucator == true)
            {
                pictureZar.Visible = true; // imaginea zarului

                positionplayer1 = rand.Next(1, 7); //generam un numar pentru jucator, intre 1 si 6
                Bitmap imagine = Properties.Resources.ResourceManager.GetObject("zar" + positionplayer1.ToString()) as Bitmap;
                imagine.MakeTransparent(); // preiau din resurse imaginea cu fata zarului conform numarului generat
                // si urmeaza sa o adaug pe ecran

                pictureZar.BackgroundImage = imagine; // adaug imaginea cu zarul
                pictureZar.BackgroundImageLayout = ImageLayout.Stretch;

                labelzarjucator1.Visible = true;
                labelzarjucator1.Text = numeJucator1 + " a dat numarul " + positionplayer1.ToString(); // afizes numarul dat de jucator

                if (i + positionplayer1 <= 99) // verific daca jucatorul poate inainta cu numarul dat de jucator
                {
                    currentPositionplayer1 = 0;
                    MutaJucator1.Start(); // evenimentul ce muta pionul pe tabla
                }

                TuraJucator = false; // schimbam tura la a da cu zarul
            }
            // verificam daca este tura celui de-al doilea jucator
            // si facem aceleasi lucru analog ca mai sus pentru 
            // tura primului jucator
            else
            {
                pictureZar.Visible = true; // imaginea zarului

                positionplayer2 = rand.Next(1, 7);  //generam un numar pentru jucator, intre 1 si 6
                Bitmap imagine = Properties.Resources.ResourceManager.GetObject("zar" + positionplayer2.ToString()) as Bitmap;
                imagine.MakeTransparent();// preiau din resurse imaginea cu fata zarului conform numarului generat
                // si urmeaza sa o adaug pe ecran

                pictureZar.BackgroundImage = imagine;// adaug imaginea cu zarul
                pictureZar.BackgroundImageLayout = ImageLayout.Stretch;

                labelzarjucator2.Visible = true;
                labelzarjucator2.Text = numeJucator2 + " a dat numarul " + positionplayer2.ToString(); // afizes numarul dat de jucator

                if (j + positionplayer2 <= 99)// verific daca jucatorul poate inainta cu numarul dat de jucator
                {
                    currentPositionplayer2 = 0; 
                    MutaJucator2.Start();// evenimentul ce muta pionul pe tabla
                }
                TuraJucator = true; // schimbam tura la a da cu zarul
            }
        }
        private void MutaJucator1Event(object sender, EventArgs e)
        {
            // aceasta este functia evenimentului ce va muta pionul
            // primului jucator pe tabla in modul de joc PvsP
            // verific daca primul pion mai are de parcurs casute
            // pana la pozitia la care are de ajuns dupa ce a dat cu zarul
            if(currentPositionplayer1 < positionplayer1)
            {
                // daca da, incrementam cu 1 la fiecare 'tick' al timer-ului
                currentPositionplayer1++;
                i++; //adaug 1 la 'i' in fiecare apel al timer-ului
                MovePiece(Jucator1, "box" + i); //modific pozitia pionului
             }
            else
            {
                //verific daca pionul a ajuns la baza unei scari sau pe capul
                //unui sarpe
                i = CheckSnakesOrLadders(i);
                //modific pozitia jucatorului in functie de raspunul apelului
                //functiei de mai sus
                MovePiece(Jucator1, "box" + i);

                //in acest moment am terminat tura primului jucator

                // afisez pozitia la care a ajuns jucatorul 1, dupa ce a dat cu zarul.
                labelpozitiejucator1.Visible = true;
                labelpozitiejucator1.Text = numeJucator1 + " se afla la pozitia " + (i + 1).ToString();
                MutaJucator1.Stop(); // opresc timer-ul
            }

            if (i == 99)
            {
                MutaJucator1.Stop();
                //daca jucatorul a ajuns la finalul table, aratam un mesaj pe ecran ce-i va transmite ca a castigat
                MessageBox.Show("Joc terminat!" + Environment.NewLine + numeJucator1 + " a castigat! " + Environment.NewLine + "Poti redeschide jocul pentru a te juca din nou!");
                //RestartGame();
            }
        }

        private void MutaJucator2Event(object sender, EventArgs e)
        {
            // aceasta este functia evenimentului ce va muta pionul
            // celui de-al doilea jucator pe tabla in modul de joc PvsP

            // verific daca pionul 2 mai are de parcurs casute
            // pana la pozitia la care are de ajuns dupa ce a dat cu zarul
            if (currentPositionplayer2 < positionplayer2)
            {
                // daca da, incrementam cu 1 la fiecare 'tick' al timer-ului
                currentPositionplayer2++;
                j++; //adaug 1 la 'i' in fiecare apel al timer-ului
                MovePiece(Jucator2, "box" + j); //modific pozitia pionului
            }
            else 
            {
                //verific daca pionul a ajuns la baza unei scari sau pe capul
                //unui sarpe
                j = CheckSnakesOrLadders(j);
                //modific pozitia jucatorului in functie de raspunul apelului
                //functiei de mai sus
                MovePiece(Jucator2, "box" + j);

                //in acest moment am terminat tura primului jucator

                // afisez pozitia la care a ajuns jucatorul 1, dupa ce a dat cu zarul.
                labelpozitiejucator2.Visible = true;
                labelpozitiejucator2.Text = numeJucator2 + " se afla la pozitia " + (j + 1).ToString();
                MutaJucator2.Stop(); // opresc timer-ul
            }
            if (j == 99)
            {
                MutaJucator2.Stop();
                //daca jucatorul 2 a ajuns la finalul tablei, aratam un mesaj pe ecran ce-i va transmite ca a castigat
                MessageBox.Show("Joc terminat!" + Environment.NewLine + numeJucator2 + " a invins! " + Environment.NewLine + "Poti redeschide jocul pentru a te juca din nou!");
                //RestartGame();
            }
        }
        private void MovePiece(PictureBox player, string posName)
        {

            //functia asta o sa mute pionii pe tabla
            //felul in care o face este simplu, am adaugat pictureboxurile tablei intr-o lista
            //astfel parcurgem lista pana ajungem la picturebox-ul dorit

            foreach (PictureBox w in Moves)
            {
                if (w.Name == posName)
                {
                    player.Location = new Point(w.Location.X + w.Width / 8, w.Location.Y + w.Height / 8);
                    player.BringToFront();
                }
            }
        }
        private int CheckSnakesOrLadders(int num)
        {
            // aceasta este functia care verifica daca jucatorul
            // se afla la baza unei scari sau pe capul unui sarpe

            // sunt mai multe secvente de tip 'if' si se poate
            // observa ca daca numarul introdus in functie 
            // se potriveste oricarei conditii 'if' acest numar
            // va fi schimbat si va fi returnat in program

            // in acest mod putem verifica in mod simplu
            // daca jucatorul a aterizat la baza unei scari
            // sa-l mutam sus unde se termina scara
            // sau daca a aterizat pe capul unui sarpe sa-l
            // mutam jos unde se termina sarpele

            if (num == 1)
            {
                num = 37;
            }

            if (num == 6)
            {
                num = 13;
            }

            if (num == 7)
            {
                num = 30;
            }

            if (num == 14)
            {
                num = 25;
            }

            if (num == 15)
            {
                num = 5;
            }
            if (num == 20)
            {
                num = 41;
            }
            if (num == 27)
            {
                num = 83;
            }
            if (num == 35)
            {
                num = 43;
            }
            if (num == 45)
            {
                num = 24;
            }
            if (num == 48)
            {
                num = 10;
            }
            if (num == 50)
            {
                num = 66;
            }
            if (num == 61)
            {
                num = 18;
            }
            if (num == 63)
            {
                num = 59;
            }
            if (num == 70)
            {
                num = 90;
            }
            if (num == 73)
            {
                num = 52;
            }
            if (num == 77)
            {
                num = 97;
            }
            if (num == 86)
            {
                num = 93;
            }
            if (num == 88)
            {
                num = 67;
            }
            if (num == 91)
            {
                num = 87;
            }
            if (num == 94)
            {
                num = 74;
            }
            if (num == 98)
            {
                num = 79;
            }
            return num;
        }

        private void InterfataPVP()
        {
            // INTERFATA JUCATOR 1

            //Imi intampin primul jucator cu un mesaj dragut
            Label LabelJucator1 = new Label();
            LabelJucator1.Location = new Point(195, 275);
            LabelJucator1.Text = "Introduceti numele primului jucator.";
            LabelJucator1.Size = new Size(780, 50);
            LabelJucator1.Font = new Font(Font.FontFamily, 16, FontStyle.Bold);
            this.Controls.Add(LabelJucator1);

            TextBox TextBoxNumeJucator = new TextBox();
            TextBoxNumeJucator.Location = new Point(285, 325);
            TextBoxNumeJucator.Size = new Size(200, 100);
            TextBoxNumeJucator.Font = new Font(Font.FontFamily, 16, FontStyle.Bold);
            this.Controls.Add(TextBoxNumeJucator);

            Button ButtonEnter = new Button();
            ButtonEnter.Location = new Point(325, 375);
            ButtonEnter.Size = new Size(125, 50);
            ButtonEnter.Text = "Enter";
            ButtonEnter.Font = new Font(Font.FontFamily, 16, FontStyle.Bold);
            this.Controls.Add(ButtonEnter);

            ButtonEnter.Click += ButtonEnter_Click;

            void ButtonEnter_Click(object sender, EventArgs e)
            {
                numeJucator1 = TextBoxNumeJucator.Text.Trim();

                //Curatenie
                this.Controls.Remove(TextBoxNumeJucator);
                this.Controls.Remove(ButtonEnter);

                LabelJucator1.Location = new Point(10, 245);
                LabelJucator1.Font = new Font(Font.FontFamily, 16, FontStyle.Bold);
                LabelJucator1.Text = "Salut " + numeJucator1 + "! Alege pionul cu care vrei sa-ti zdrobesti dusmanul!";

                Button[] pioni = new Button[5];
                for (int i = 0; i < 5; i++)
                {
                    pioni[i] = new Button();
                    pioni[i].Location = new Point(150 * i + 50, 325);
                    pioni[i].Size = new Size(100, 100);
                    pioni[i].BackgroundImage = Properties.Resources.ResourceManager.GetObject(NumePion[i]) as Image;
                    pioni[i].Tag = i;
                    pioni[i].BackgroundImageLayout = ImageLayout.Stretch;
                    this.Controls.Add(pioni[i]);

                    pioni[i].Click += (sender1, e1) =>
                    {
                        // Pastrez pionul pe care l-a selectat jucatorul
                        i = (int)((Button)sender1).Tag;
                        pionJucator1 = Properties.Resources.ResourceManager.GetObject(NumePion[i] + "joc") as Image;

                        //Elimin din vectorul NumePion numele pionului ales de primul jucator
                        int poz = (int)((Button)sender1).Tag;
                        for (int j = poz; j < 4; j++)
                            NumePion[j] = NumePion[j + 1];

                        //Curatenie
                        this.Controls.Remove(LabelJucator1);
                        foreach (Button pion in pioni)
                            this.Controls.Remove(pion);

                        //Imi intampin urmatorul jucator tot cu un mesaj dragut
                        Label LabelJucator2 = new Label();
                        LabelJucator2.Location = new Point(175, 275);
                        LabelJucator2.Text = "Introduceti numele celui de-al doilea jucator.";
                        LabelJucator2.Size = new Size(800, 50);
                        LabelJucator2.Font = new Font(Font.FontFamily, 16, FontStyle.Bold);
                        this.Controls.Add(LabelJucator2);

                        TextBox TextBoxNumeJucator2 = new TextBox();
                        TextBoxNumeJucator2.Location = new Point(285, 325);
                        TextBoxNumeJucator2.Size = new Size(200, 100);
                        TextBoxNumeJucator2.Font = new Font(Font.FontFamily, 16, FontStyle.Bold);
                        this.Controls.Add(TextBoxNumeJucator2);

                        Button ButtonEnter2 = new Button();
                        ButtonEnter2.Location = new Point(325, 375);
                        ButtonEnter2.Size = new Size(125, 50);
                        ButtonEnter2.Text = "Enter";
                        ButtonEnter2.Font = new Font(Font.FontFamily, 16, FontStyle.Bold);
                        this.Controls.Add(ButtonEnter2);

                        ButtonEnter2.Click += (sender2, e2) =>
                        {
                            numeJucator2 = TextBoxNumeJucator2.Text.Trim();

                            //Curatenie
                            this.Controls.Remove(TextBoxNumeJucator2);
                            this.Controls.Remove(ButtonEnter2);

                            LabelJucator2.Location = new Point(20, 245);
                            LabelJucator2.Font = new Font(Font.FontFamily, 14, FontStyle.Bold);
                            LabelJucator2.Text = "Salut " + numeJucator2 + "! Alege pionul cu care vrei sa-l infrangi pe " + numeJucator1 +
                            ", dusmanul tau de moarte!";

                            for (i = 0; i < 4; i++)
                            {
                                pioni[i] = new Button();
                                pioni[i].Location = new Point(150 * i + 100, 325);
                                pioni[i].Size = new Size(100, 100);
                                pioni[i].Tag = i;
                                pioni[i].BackgroundImage = Properties.Resources.ResourceManager.GetObject(NumePion[i]) as Image;
                                pioni[i].BackgroundImageLayout = ImageLayout.Stretch;
                                this.Controls.Add(pioni[i]);

                                pioni[i].Click += (sender3, e3) =>
                                {
                                    // Pastrez pionul pe care l-a selectat jucatorul
                                    i = (int)((Button)sender3).Tag;
                                    pionJucator2 = Properties.Resources.ResourceManager.GetObject(NumePion[i] + "joc") as Image;

                                    //Curatenie
                                    this.Controls.Remove(LabelJucator2);
                                    foreach (Button pion in pioni)
                                      this.Controls.Remove(pion);

                                    // INCEPE JOCUL
                                    SetupGamePVP();

                                };
                            }
                        };

                    };
                }

            }
        }

        private void buttonPvsP_Click(object sender, EventArgs e)
        {
            // Fac invizibile butoanele utilizate anterior
            // in interfata initiala
            buttonPvsP.Visible = false;
            buttonPvsPC.Visible = false;

            //Preiau datele celor 2 jucatori
            InterfataPVP();
        }
        private void InterfataPvsPC()
        {
            // INTERFATA JUCATOR 1

            //Imi intampin jucatorul cu un mesaj dragut
            Label LabelJucator1 = new Label();
            LabelJucator1.Location = new Point(255, 275);
            LabelJucator1.Text = "Introduceti numele jucatorului.";
            LabelJucator1.Size = new Size(850, 50);
            LabelJucator1.Font = new Font(Font.FontFamily, 16, FontStyle.Bold);
            this.Controls.Add(LabelJucator1);

            TextBox TextBoxNumeJucator = new TextBox();
            TextBoxNumeJucator.Location = new Point(325, 325);
            TextBoxNumeJucator.Size = new Size(200, 100);
            TextBoxNumeJucator.Font = new Font(Font.FontFamily, 16, FontStyle.Bold);
            this.Controls.Add(TextBoxNumeJucator);

            Button ButtonEnter = new Button();
            ButtonEnter.Location = new Point(355, 375);
            ButtonEnter.Size = new Size(125, 50);
            ButtonEnter.Text = "Enter";
            ButtonEnter.Font = new Font(Font.FontFamily, 16, FontStyle.Bold);
            this.Controls.Add(ButtonEnter);

            ButtonEnter.Click += ButtonEnter_Click;

            void ButtonEnter_Click(object sender, EventArgs e)
            {
                numeJucator1 = TextBoxNumeJucator.Text.Trim();

                //Curatenie
                this.Controls.Remove(TextBoxNumeJucator);
                this.Controls.Remove(ButtonEnter);

                LabelJucator1.Location = new Point(10, 245);
                LabelJucator1.Text = "Salut " + numeJucator1 + "! Alege pionul cu care vrei sa-ti zdrobesti dusmanul!";

                Button[] pioni = new Button[5];
                for (int i = 0; i < 5; i++)
                {
                    pioni[i] = new Button();
                    pioni[i].Location = new Point(150 * i + 50, 325);
                    pioni[i].Size = new Size(100, 100);
                    pioni[i].BackgroundImage = Properties.Resources.ResourceManager.GetObject(NumePion[i]) as Image;
                    pioni[i].Tag = i;
                    pioni[i].BackgroundImageLayout = ImageLayout.Stretch;
                    this.Controls.Add(pioni[i]);

                    pioni[i].Click += (sender1, e1) =>
                    {
                        // Pastrez pionul pe care l-a selectat jucatorul
                        i = (int)((Button)sender1).Tag;
                        pionJucator1 = Properties.Resources.ResourceManager.GetObject(NumePion[i] + "joc") as Image;

                        //Elimin din vectorul NumePion numele pionului ales de primul jucator
                        int poz = (int)((Button)sender1).Tag;
                        for (int j = poz; j < 4; j++)
                            NumePion[j] = NumePion[j + 1];

                        //Curatenie
                        this.Controls.Remove(LabelJucator1);
                        foreach (Button pion in pioni)
                            this.Controls.Remove(pion);
                        // INCEPE JOCUL
                        numeJucator2 = "Calculatorul"; // In variabila pentru numele celui de-al doilea jucator, pun numele "Calculatorul"
                        SetupGamePvsPC();
                    };
                }

            }
        }

        private void SetupGamePvsPC()
        {
            this.BackgroundImage = Properties.Resources.fundalform;
            // In aceasta functie vom face tabla, cu cei 2 jucatori
            buttonZarPvsPC.Visible = true; // butonul de aruncat cu zaru
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            int leftPos = 50; //leftpos o sa ne ajuta in a pozitiona casetele de la dreapta la stang 
            int topPos = 600; //toppos o sa ne ajuta sa punem casetele de jos in sus
            int a = 0; // a ne va ajuta sa punem 10 casete in linie

            for (int i = 0; i < 100; i++)
            {
                Image ImagineCaseta; // o variabila ce va atasa casetelor imagini pentru a forma tabla
                ImagineCaseta = Properties.Resources.ResourceManager.GetObject("_" + i.ToString()) as Image;

                // cream o caseta de tip picturebox de forma patrata
                PictureBox box = new PictureBox();
                box.BackgroundImage = ImagineCaseta;
                box.BackgroundImageLayout = ImageLayout.Stretch;
                box.Size = new Size(60, 60);

                // numim casetele
                box.Name = "box" + i.ToString(); // numele casetelor

                Moves.Add(box); //adaug casetele noi in lista de casete

                // dedesubt vom face algoritmul de care avem nevoie sa punem 10 casete in linie
                // punem casetele de la stanga la dreapta si apoi mutam in sus si inversam procesul
                // "a" controleaza cum pozitionam casetele 

                if (a == 10)
                {
                    // asta se intampla cand punem 10 casete de la stanga la dreapta
                    topPos -= 60; // in acest caz scadem 60 din "toppos" astfel incat sa punem urmatoarele casete pe un rand mai sus 
                    a = 30; // schimbam valoarea lui a la 30, facem asta pentru a pune casetele de la dreapta la stang acum
                }

                if (a == 20)
                {
                    // asta se intampla cand punem 10 casete de la dreapta la stanga
                    topPos -= 60; // din nou scadem 60 pentru a trece la un rand mai sus
                    a = 0; // setam a din nou la valorea 0
                }

                if (a > 20)
                {
                    // daca valoarea lui a este mai mare de 20
                    // punem casete de la dreapta la stanga
                    a--; //scadem 1 din a pentru fiecare caseta pe care o punem de la dreapta la stanga
                    box.Location = new Point(leftPos, topPos); // punem caseta pe pozitii
                    leftPos -= 60; //scadem 60 pentru a pune fiecare caseta de la dreapta la stanga
                }

                if (a < 10)
                {
                    // asta se intampla cand vrem sa punem casete de la stanga la dreapta
                    // daca a este mai mic decat 10

                    a++; //adaug 1 la a pentru fiecare caseta pe care o punem de la stanga la dreapta
                    leftPos += 60; //adaug 60 la leftpos pentru fiecare caseta pe care o punem de la stanga la dreapta
                    box.Location = new Point(leftPos, topPos); // punem caseta pe pozitii
                }
                this.Controls.Add(box); //in final adaugam caseta in interfata
            }

            MutaJucator.Tick += MutaJucatorEvent;
            MutaJucator.Interval = 285;

            //facem imaginile cu pionii jucatorilor si le asezam la pozitia de start
            //PION JUCATOR 1

            Jucator1 = new PictureBox();
            Jucator1.BackgroundImage = pionJucator1;
            Jucator1.BackgroundImageLayout = ImageLayout.Stretch;
            Jucator1.Size = new Size(50, 50);
            Jucator1.BackColor = Color.Transparent;
            this.Controls.Add(Jucator1);

            pictureBox1.BackgroundImage = pionJucator1;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.BackColor = Color.Transparent;

            //PION JUCATOR 2

            // pionul ales pentru calculator va fi unul la intamplare
            // dintre cele ramase dupa ce primul jucator isi va fi ales

            pionJucator2 = Properties.Resources.ResourceManager.GetObject(NumePion[rand.Next(1,4)] + "joc") as Bitmap;

            Jucator2 = new PictureBox();
            Jucator2.BackgroundImage = pionJucator2;
            Jucator2.BackgroundImageLayout = ImageLayout.Stretch;
            Jucator2.Size = new Size(50, 50);
            Jucator2.BackColor = Color.Transparent;
            this.Controls.Add(Jucator2);

            pictureBox2.BackgroundImage = pionJucator2;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.BackColor = Color.Transparent;

            MovePiece(Jucator1, "box0");
            MovePiece(Jucator2, "box0");
        }

        private void buttonZarPvsPC_Click(object sender, EventArgs e)
        {
            // acest eveniment este atasat butonului
            // "Arunca zarul", astfel incat se declanseaza
            // cand jucatorul apasat acest buton
            labelpozitiejucator1.Visible = true;
            labelpozitiejucator2.Visible = true;
            labelzarjucator1.Visible = true;
            labelzarjucator2.Visible = true;
            pictureZar.Visible = true;

            if (TuraJucator1 == false && TuraJucator2 == false)
            {
                positionplayer1 = rand.Next(1, 7); //generam un numar pentru zar
                Bitmap imagine = Properties.Resources.ResourceManager.GetObject("zar" + positionplayer1.ToString()) as Bitmap;
                imagine.MakeTransparent();
                pictureZar.BackgroundImage = imagine; // aratam imaginea cu numarul generat pentru zar
                pictureZar.BackgroundImageLayout = ImageLayout.Stretch;
                labelzarjucator1.Text = numeJucator1 + " a dat numarul " + positionplayer1.ToString();//aratam numarul
                currentPositionplayer1 = 0; //setam pozitia curenta ca 0


                //verificam daca jucatorul poate inainta pe tabla
                if ((i + positionplayer1) <= 99)
                {

                    TuraJucator1 = true; //marcam faptul ca jucatorul poate inainte
                    MutaJucator.Start(); // evenimentul ce muta pionul
                }
                else
                {
                    if (TuraJucator2 == false)
                    {
                        TuraJucator2 = true;
                        positionplayer2 = rand.Next(1, 7); //generam numar pentru zarul PC-ului si-l aratam
                        imagine = Properties.Resources.ResourceManager.GetObject("zar" + positionplayer2.ToString()) as Bitmap;
                        imagine.MakeTransparent();
                        pictureZar.BackgroundImage = imagine;
                        pictureZar.BackgroundImageLayout = ImageLayout.Stretch;
                        labelzarjucator2.Text = numeJucator2 + " a dat numarul " + positionplayer2.ToString();
                        currentPositionplayer2 = 0; // initializam cu 0 numarul curent de pasi pe care urmeaza sa-i faca pionul
                        MutaJucator.Start(); //pornim functia ce muta pionii
                    }
                    else
                    {
                        // if the player two round is already true then
                        // daca tura calculatorului este deja facuta atunci
                        MutaJucator.Stop(); //oprim timer-ul
                        //schimbam cele 2 variabila ca false
                        TuraJucator1 = false;
                        TuraJucator2 = false;
                    }
                }
            }
        }

        private void MutaJucatorEvent(object sender, EventArgs e)
        {
            // acesta este eveniment timer-ului ce va misca
            // pionii jucatorului si calculatorului pe tabla
            
            if (TuraJucator1 == true && TuraJucator2 == false)
            {
                // verificam daca pozitia curenta a jucatorului
                // este mai mica decat pozitia la care trebuie sa ajunga
                // jucatorul 
                if (currentPositionplayer1 < positionplayer1)
                {
                    // daca da, adaugam 1 la pozitia curenta de fiecare data
                    currentPositionplayer1++;
                    i++; // adaugam 1 la 'i'(pozitia pionului jucatorului pe tabla)
                    MovePiece(Jucator1, "box" + i); //actualizam pozitia jucatorului
                }
                else
                {
                    TuraJucator2 = true; 
                    i = CheckSnakesOrLadders(i); // verific daca jucatorul a ajung la baza unei scari sau pe capul unui sarpe
                    MovePiece(Jucator1, "box" + i); // actualizez pozitia acestuia pe tabla, daca e cazul

                    // am terminat runda jucatorului, acum urmeaza calculatorul

                    x = positionplayer2 = rand.Next(1, 7); //generez un numar pentru PC pe care il afisez ca imagine si ca text
                    Bitmap imagine = Properties.Resources.ResourceManager.GetObject("zar" + positionplayer2.ToString()) as Bitmap;
                    imagine.MakeTransparent();
                    pictureZar.BackgroundImage = imagine;
                    pictureZar.BackgroundImageLayout = ImageLayout.Stretch;
                    labelzarjucator2.Text = numeJucator2 + " a dat numarul " + positionplayer2.ToString(); 
                    currentPositionplayer2 = 0; //setez pozitia curenta a calculatorui
                    labelpozitiejucator1.Text = numeJucator1 + " este la pozitia " + (i + 1).ToString(); // afisez pozitia jucatorului pe tabla
                }

                //verific daca jucatorul a ajuns la finalul tablei de joc
                if (i == 99)
                {
                    // daca da, opresc jocul si afisez un mesaj simpatic
                    MutaJucator.Stop();
                    MessageBox.Show("Joc terminat!" + Environment.NewLine + numeJucator1 + " a castigat! " + Environment.NewLine + "Poti redeschide jocul pentru a te juca din nou!");
                }
            }

            // ce urmeaza mai jos este partea pentru calculator,
            // aceasta va functiona doar cand variabila
            // TuraJucator2 este setata ca adevarata
            if (TuraJucator2 == true)
            {
                // la fel ca in cazul jucatorului, verific daca
                // pionul poate inainta
                if (currentPositionplayer2 < positionplayer2 && (j + x <= 99))
                {

                    currentPositionplayer2++; // incrementez numarul de pasi pe care-i face pionul calculatorului
                    j++; // incrementez pozitia la care se muta pionul
                    x--; // decrementez pe x astfel incat j + x sa fie in continuare pozitia corecta la care trebuie sa ajunga calculatorul
                    MovePiece(Jucator2, "box" + j); //actualizez pozitia pionului calculatorului
                }
                else
                {
                    //dupa ce calculatorul isi foloseste tura
                    //setez cele 2 variabile ca false
                    TuraJucator1 = false;
                    TuraJucator2 = false;
                    //verific daca pionul trebuie mutat mai sus sau mai jos
                    j = CheckSnakesOrLadders(j);
                    MovePiece(Jucator2, "box" + j);
                    // afisez pozitia la care a ajuns calculatorul
                    labelpozitiejucator2.Text = numeJucator2 + " este la pozitia " + (j + 1).ToString();
                    // oprim functia evenimentul ce muta pionii
                    MutaJucator.Stop();
                }
                //daca calculatorul a ajuns la final, terminam jocul
                if (j == 99)
                {
                    //oprim functia ce muta pionii, si afisam un mesaj
                    MutaJucator.Stop();
                    MessageBox.Show("Joc Terminat!" + Environment.NewLine + numeJucator2 + " a invins!" + Environment.NewLine + "Redeschide jocul pentru a te juca din nou!");
                }
            }
        }

        private void buttonPvsPC_Click(object sender, EventArgs e)
        {
            // Fac invizibile butoanele utilizate anterior
            // in interfata initiala
            buttonPvsP.Visible = false;
            buttonPvsPC.Visible = false;

            //Preiau datele celor 2 jucatori
            InterfataPvsPC();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
