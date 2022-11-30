using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;

namespace Kurs_Gorbatyuk_Anastasiya_931903
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int ncub; // количество прямоугольников
        public int kWidth, kHeight, kAreal;
        public int areaSumCub = 0; // площадь покрывающего прямоугольника
        public double kEffect; // коэффициент эффективности       

        // для генетического алгоритма
        public int maxpop = 100; // максимум в популяции
        public int maxstring = 10; // битовая строк
        public int numpop; // число особей в поколении
        public double pmutation; // вероятность мутации
        public int gen; // номер рассматриваемого поколения
        public int numgen; // число поколений
        public double numCrossof;
        public double[] maxmass;
        public double[] sredmass;


        public class PointC
        {
            public int x, y;
            public PointC(int _x, int _y)
            {
                x = _x;
                y = _y;
            }
        }
        public class RectagleC
        {
            public int h, w;
            public PointC leftUpPoint;
            public PointC rightUpPoint;
            public PointC leftDownPoint;
            public PointC rightDownPoint;
            public int number;

            public RectagleC()
            {
                h = 0;
                w = 0;
                leftUpPoint = null;
                rightUpPoint = null;
                leftDownPoint = null;
                rightDownPoint = null;
            }
            public RectagleC(int _w, int _h, int number)
            {
                this.h = _h;
                this.w = _w;
                leftUpPoint = null;
                rightUpPoint = null;
                leftDownPoint = null;
                rightDownPoint = null;
                this.number = number;
            }


        } //прямоугольники
        public class RectLocation // резмещение прямоугольников
        {
            public List<RectagleC> rects= new List<RectagleC>();
            public int arealPP;
            public bool lucky;

            
            public RectLocation()
            {
                arealPP = 0;
            }
        }
            public int xar;
            public int yar;
        public int arealSearch (RectLocation pop)
        {
            xar = 0;
            yar = 0;
            for (int i = 0; i < pop.rects.Count; i++)
            {
                if (pop.rects[i].rightDownPoint.x >= xar)
                {
                    xar = pop.rects[i].rightDownPoint.x;
                }
                if (pop.rects[i].rightDownPoint.y >= yar)
                {
                    yar = pop.rects[i].rightDownPoint.y;
                }
            }
            return (xar * yar);
        }

        public class Positions
        {
            public int arealCoverMaybe;
            public PointC PStop;

            public Positions(int arealCoverMaybe, PointC pStop)
            {
                this.arealCoverMaybe = arealCoverMaybe;
                PStop = pStop;
            }

            public Positions()
            {
                this.arealCoverMaybe = 0;


            }
        }
        //список цветов
        List <Color> colors= new List<Color>() { Color.Yellow, Color.Green, Color.Blue , Color.Red, Color.Aqua, Color.Violet, Color.Lime, Color.Maroon, Color.DarkBlue, Color.DarkGreen, Color.GreenYellow, Color.Coral, Color.Purple, Color.Cornsilk, Color.Gray, Color.DarkKhaki, Color.MistyRose, Color.Teal, Color.DarkSlateBlue, Color.Indigo, Color.Navy, Color.Crimson};
        List<RectagleC> rectsProb = new List<RectagleC>(); // пробный
        
        List<RectagleC> startPop = new List<RectagleC>();//создаем список для прямоугольников
        RectLocation startPopLoc = new RectLocation();

       // List<RectagleC> finishPop = new List<RectagleC>(); // лучшая популяция, результат, представленный пользоватлю 

        // для ГА
        List<RectLocation> oldpop = new List<RectLocation>(); // старое поколение популяции
        List<RectLocation> newpop = new List<RectLocation>(); // новое поколение популяции
        List<RectLocation> intpop = new List<RectLocation>(); // полонение для сортировки 
        RectLocation BestOsob = new RectLocation(); //лучшая особь
        public double bestEffect = 0; // коэф еффективности лучшей особи
        List<RectLocation> BestOsobStat = new List<RectLocation>(); // полонение для сортировки 

        private void btStart_Click(object sender, EventArgs e)
        {
            RectLocation rectsProbLoc = new RectLocation();
            ncub = 11;
            RectagleC a1 = new RectagleC(1, 2,0); //2
            rectsProb.Add(a1);
            RectagleC a2 = new RectagleC(3, 3,1); //9
            rectsProb.Add(a2);
            RectagleC a3 = new RectagleC(1, 2,2);//2
            rectsProb.Add(a3);
            RectagleC a4 = new RectagleC(3, 2,3);//6
            rectsProb.Add(a4);
            RectagleC a5 = new RectagleC(2, 1,4);//2
            rectsProb.Add(a5);
            RectagleC a6 = new RectagleC(3, 1,5);//3
            rectsProb.Add(a6);
            RectagleC a7 = new RectagleC(2, 1,6);//2
            rectsProb.Add(a7);
            RectagleC a8 = new RectagleC(3, 2,7);//6
            rectsProb.Add(a8);
            RectagleC a9 = new RectagleC(1, 3,8);//3
            rectsProb.Add(a9);
            RectagleC a10 = new RectagleC(2, 2,9);//4
            rectsProb.Add(a10);
            RectagleC a11 = new RectagleC(2, 2,10);//4
            rectsProb.Add(a11);
            
            rectsProbLoc.rects = rectsProb;

            kWidth = (int)numWidth.Value; // значение ширины контейнера
            kHeight = (int)numHeight.Value; //значение высоты контейнера
            kAreal = kWidth * kHeight;

            // СЧИТЫВАНИЕ ПАРАМЕТРОВ ГА
            numpop = (int)numPopulation.Value;
            numgen = (int)numGen.Value;
            numCrossof = (double)numCross.Value;
            pmutation=(double)numMut.Value;

            //добавляем в список введеные параметры прямоугольников
            
            for (int i = 0; i < startPop.Count; i++)
            {
                RectagleC rct = new RectagleC(Convert.ToInt32(dataGridView1[0, i].Value.ToString()), Convert.ToInt32(dataGridView1[1, i].Value.ToString()),i);
                if (rct.h > kHeight || rct.w > kWidth)
                {
                    Error2WH();
                }
                //startPop.Add(rct); //ВЕРНУТЬ!!!
            }
            //пробное, УДАЛИТЬ!!!
            startPopLoc = rectsProbLoc;         

            for (int i = 0; i < ncub; i++)
            {
                areaSumCub += startPopLoc.rects[i].h * startPopLoc.rects[i].w;
            }


            if (areaSumCub > kHeight * kWidth)
            {
                Error1Area();
            } 
            else
            {
                double effect = 0;
                int arealPP = 0;
                double qmax = 0;
                double psred = 0;
                // НАЧАЛО ГА. ПЕРВОЕ РАЗМЕЩЕНИЕ В STARTPOP
                Initpop();
                gen = 0;
                maxmass = new double[numgen];
                sredmass = new double[numgen];
                int p = 0;
                for (int i = 0; i < numpop; i++)
                {
                   effect = putRectagle(oldpop[i]); // коэффициент эффективности 
                    
                    //Console.WriteLine($"Особь {i} КЕ {effect}");
                    if (effect > qmax) 
                    {
                        qmax = effect;
                        p = i;
                    }
                    psred += effect;
                }
                sredmass[0] = psred / (double)numpop;
                maxmass[0] = qmax;
                bestEffect = qmax;
                BestOsob = oldpop[p];
                Console.WriteLine(BestOsob.arealPP);

                //PaintChartRectagle(BestOsob);

                if (BestOsob.arealPP==0)
                {
                    Error3NotPP();
                }

                chart1.Series[0].Points.AddXY(gen, maxmass[0]);
                chart1.Series[1].Points.AddXY(gen, sredmass[0]);

                for (int i = 1; i < numgen; i++)
                {
                    gen++;
                    generation(); // создаем новое поколение
                    chart1.Series[0].Points.AddXY(gen, maxmass[i]);
                    chart1.Series[1].Points.AddXY(gen, sredmass[i]);
                    oldpop.Clear();
                    for (int j = 0; j < newpop.Count; j++)
                    {
                        oldpop.Add(newpop[j]);
                    }
                    intpop.Clear();
                    newpop.Clear();
                }

                // КОНЕЦ ГА. РЕЗУЛЬТАТ В FINISHPOP

                lbPPP.Text = BestOsob.arealPP.ToString(); //ППП
                lbKE.Text= Math.Round(bestEffect, 3).ToString(); // коэффициент эффективности
                lbArealKon.Text = kAreal.ToString(); // площадь контейнера
                lbSumRec.Text = areaSumCub.ToString(); // сумма площадей прямоугольников

                PaintChartRectagle(BestOsob);

                
            }                    
        }

        public void Error1Area()
        {
            Form form = new Form2(); 
            form.Show();
            dataGridView1.Rows.Clear();            
            startPop.Clear();
            ncub = 0;
            areaSumCub = 0;
        }

        public void Error2WH()
        {
            Form form = new Form2();
            form.Show();
            dataGridView1.Rows.Clear();
            startPop.Clear();
            ncub = 0;
            areaSumCub = 0;
        }
        public void Error3NotPP()
        {
            Form form = new Form3();
            form.Show();
            dataGridView1.Rows.Clear();
            startPop.Clear();
            ncub = 0;
            areaSumCub = 0;
        }

        public double putRectagle(RectLocation pop)
        {
            //создаем булевый массив по размеру контейнера 
            bool[,] massKont = new bool[kHeight, kWidth];
            for (int i = 0; i < kHeight; i++)
            {
                for (int j = 0; j < kWidth; j++)
                {
                    massKont[i, j] = false;
                }
            }

            int[,] massKontV = new int[kHeight, kWidth];
            

            // ставим первый прямоугольник в контейнер
            PointC lu= new PointC(0,0);
            PointC ru = new PointC(pop.rects[0].w, 0);
            PointC rd = new PointC(pop.rects[0].w,pop.rects[0].h);
            PointC ld = new PointC(0, pop.rects[0].h);

            pop.rects[0].leftUpPoint=lu;
            pop.rects[0].rightUpPoint = ru;
            pop.rects[0].rightDownPoint = rd;
            pop.rects[0].leftDownPoint = ld;
            
            for (int i = 0; i < pop.rects[0].h; i++)
            {
                for (int j = 0; j < pop.rects[0].w; j++)
                {
                    massKont[i, j] = true;
                }
            }
            List<Positions> positions = new List<Positions>();
            PointC PointLU;
            //Console.WriteLine($"Номер прямоугольника: 0  x: 0  y: 0  ППП: y: {pop[0].h * pop[0].w}");

            // вычисляем площадь покрытия 
            int arealCoverNow = pop.rects[0].w * pop.rects[0].h; // площадь покрывающего прямоугольника сейчас
            int xArealCoverNow = pop.rects[0].w;// ширина покрывающего прямоугольника сейчас
            int yArealCoverNow = pop.rects[0].h; // высота покрывающего прямоугольника сейчас
            bool numIntersection = true; // наличие пересечений
            int arealCoverMaybe = 0; // площадь покрывающего прямоугольника рассматриваемого варианта
            int xArealCoverMaybe = 0;// ширина покрывающего прямоугольника рассматриваемого варианта
            int yArealCoverMaybe = 0; // высота покрывающего прямоугольника рассматриваемого варианта
            int b = kHeight * kWidth; // для поиска наилучшего положения
            Positions BestPos = new Positions();
            RectagleC rt;


            int BestAreal = kHeight * kWidth;

            for (int i = 1; i < pop.rects.Count; i++) //для каждого прямоугольника
            {
                //Console.WriteLine($"Прямоугольник: {i}  ширина: {pop[i].w}  высота: {pop[i].h}");
                //проходим каждую секцию покрывающего прямоугольника+1 справа и снизу
                for (int j = 0; j < yArealCoverNow + 1; j++)
                {
                    for (int k = 0; k < xArealCoverNow + 1; k++)
                    {
                        //Console.WriteLine($"Позиция:  х={k}  y={j}");
                        if ((k + pop.rects[i].w) <= kWidth) // если прямоугольник не выходит по х
                        {
                            if (j + pop.rects[i].h <= kHeight) // если прямоугольник не выходит по у
                            {
                                // проверяем пересечения
                                for (int l = 0; l < pop.rects[i].h; l++)
                                {
                                    for (int p = 0; p < pop.rects[i].w; p++)
                                    {
                                        if (massKont[j + l, k + p]) // если в клетке есть фигура, то место занято
                                        {
                                            numIntersection = false;
                                        }
                                    }
                                }

                                if (numIntersection) //если пересечений не обнаружено
                                {
                                    // высчитываем площадь покрывающего прямоугольника с учетом нового прямоугольника
                                    if ((j + pop.rects[i].h) > yArealCoverNow)
                                    {
                                        yArealCoverMaybe = j + pop.rects[i].h;
                                    }
                                    else
                                    {
                                        yArealCoverMaybe = yArealCoverNow;
                                    }

                                    if ((k + pop.rects[i].w) > xArealCoverNow)
                                    {
                                        xArealCoverMaybe = k + pop.rects[i].w;
                                    }
                                    else
                                    {
                                        xArealCoverMaybe = xArealCoverNow;
                                    }
                                    arealCoverMaybe = xArealCoverMaybe * yArealCoverMaybe;
                                   // Console.WriteLine($"ППП:  х={xArealCoverMaybe}  y={yArealCoverMaybe}");

                                    //Добавляем возможную позицию в список
                                    PointLU = new PointC(k, j);
                                    Positions o = new Positions(arealCoverMaybe, PointLU);
                                    positions.Add(o);
                                }
                                else
                                {
                                   // Console.WriteLine("Есть пересечения");
                                }
                            }
                            else
                            {
                               // Console.WriteLine("Выход за границу контейнера по высоте");
                            }
                        }
                        else
                        {
                            //Console.WriteLine("Выход за границу контейнера по ширине");
                        }
                        numIntersection = true;
                    }

                }

                if (positions.Count == 0)
                {
                    pop.lucky = false;
                    return 0;
                    // если следующий прямоугольник поставить в контейнер не получается
                }

                //ищем лучшую позицию
                //Console.WriteLine("Поиск лучшей позиции");
                BestPos = positions[0];
                b = positions[0].arealCoverMaybe;
                if (positions.Count > 0)
                {
                    for (int j = 1; j < positions.Count; j++)
                    {
                       // Console.WriteLine($"Позиция: {j}  x: {positions[j].PStop.x}  y: {positions[j].PStop.y}  ППП: {positions[j].arealCoverMaybe}");
                        if (positions[j].arealCoverMaybe <= b)
                        {
                            //Console.WriteLine("    ППП позиции меньше или равна ППП лучшей позиции");
                            if (positions[j].arealCoverMaybe == b)
                            {
                                //Console.WriteLine("    ППП позиции равна ППП лучшей позиции");
                                if (positions[j].PStop.y < BestPos.PStop.y)
                                {
                                   // Console.WriteLine("    у позиции меньше у лучшей позиции. ПОЗИЦИЯ ВЫБРАНА");
                                    BestPos = positions[j];
                                    b = positions[j].arealCoverMaybe;
                                }
                                else
                                {
                                    if (positions[j].PStop.x < BestPos.PStop.x)
                                    {
                                       // Console.WriteLine("    x позиции меньше x лучшей позиции. ПОЗИЦИЯ ВЫБРАНА");
                                        BestPos = positions[j];
                                        b = positions[j].arealCoverMaybe;
                                    }
                                }
                            }
                            else
                            {
                               // Console.WriteLine("    ППП позиции меньше ППП лучшей позиции ПОЗИЦИЯ ВЫБРАНА");
                                BestPos = positions[j];
                                b = positions[j].arealCoverMaybe;
                            }
                        }
                    }
                }


                // добавляем прямоугольник в контейнер
                for (int j = 0; j < pop.rects[i].h; j++)
                {
                    for (int k = 0; k < pop.rects[i].w; k++)
                    {
                        massKont[BestPos.PStop.y + j, BestPos.PStop.x + k] = true;
                    }
                }
                // высчитываем площадь покрывающего прямоугольника с учетом нового прямоугольника
                if ((BestPos.PStop.y + pop.rects[i].h) > yArealCoverNow)
                {
                    yArealCoverNow = BestPos.PStop.y + pop.rects[i].h;
                }
                if ((BestPos.PStop.x + pop.rects[i].w) > xArealCoverNow)
                {
                    xArealCoverNow = BestPos.PStop.x + pop.rects[i].w;
                }

                arealCoverNow = xArealCoverNow * yArealCoverNow;
                // расставлям точки
              
                lu = new PointC(BestPos.PStop.x, BestPos.PStop.y);
                ru = new PointC(BestPos.PStop.x + pop.rects[i].w, BestPos.PStop.y);
                rd = new PointC(BestPos.PStop.x + pop.rects[i].w, BestPos.PStop.y + pop.rects[i].h);
                ld = new PointC(BestPos.PStop.x, BestPos.PStop.y + pop.rects[i].h);

                pop.rects[i].leftUpPoint = lu;
                pop.rects[i].rightUpPoint = ru;
                pop.rects[i].rightDownPoint = rd;
                pop.rects[i].leftDownPoint = ld;

                positions.Clear();
                b = kHeight * kWidth;         
            }           
          
            pop.lucky = true;
            pop.arealPP = arealSearch(pop);
            return ((double)areaSumCub/(double)pop.arealPP);

        }

        private void btParamCub_Click(object sender, EventArgs e)
        {
            ncub = (int)numCub.Value;
            for (int i = 0; i < ncub; i++)
            {
                dataGridView1.Rows.Add();
            }
        }

        public void Initpop()
        {
            Random rnd = new Random();
            List<int> n = new List<int>();
            int p;
            for (int i=0; i < ncub; i++)
            {                
                n.Add(i);
            }            
            
            for (int i = 0; i < numpop; i++) // перебираем все варианты размещения
            {
                //Console.WriteLine($"Особь {i}");
                List<RectagleC> q = new List<RectagleC>(); 
                for (int j = 0; j < ncub; j++) 
                {                  
                    p = rnd.Next(0, n.Count);
                    RectagleC c = new RectagleC(startPopLoc.rects[n[p]].w, startPopLoc.rects[n[p]].h, startPopLoc.rects[n[p]].number);

                    q.Add(c);
                    n.RemoveAt(p);
                    //Console.WriteLine($" Прямоугольник {j}. Ширина: {q[j].w}. Высота {q[j].h}");                    
                }
                RectLocation h= new RectLocation();
                h.rects = q;
                oldpop.Add(h);
                n.Clear();
                for (int k = 0; k < ncub; k++)
                {
                    n.Add(k);
                }                  
            }
            //for (int i = 0; i < oldpop.Count; i++)
            //{
            //    Console.WriteLine($"Особь {i}");
            //    for (int j = 0; j < oldpop[i].Count; j++)
            //    {
            //        Console.WriteLine($" Прямоугольник {j}. Ширина: {oldpop[i][j].w}. Высота {oldpop[i][j].h}");
            //    }
            //}
        }

        public void generation() // создаем новое поколение
        {
            select(); // сортируем старое поколение, отсортированное поколение в intpop турнирный отбор=2
           
            for (int i = 0; i < numpop; i+=2) // создаем потомков
            {
                crossover(intpop[i], intpop[i + 1]);

            }
            
            int k=0;
            while (newpop.Count < numpop) // заполняем новое поколение оставшимися лучшими старыми особями
            {
                newpop.Add(intpop[k]);
                k++;
            }
            int p = 0;
            double effect;
            double qmax = 0;
            double psred = 0;
            
            for (int i = 0; i < numpop; i++)
            {
                effect = putRectagle(newpop[i]); // коэффициент эффективности 
                                                 //Console.WriteLine($"Особь {i} КЕ {effect}");
                if (effect > qmax)
                {
                    qmax = effect;
                    p = i;
                }
                psred += effect;
            }           
            maxmass[gen] = qmax;
            sredmass[gen] = psred / (double)numpop;
            if (bestEffect < qmax)
            {
                bestEffect = qmax;
                BestOsob = newpop[p];
                Console.WriteLine(BestOsob.arealPP);
            }


            for (int i = 0; i < numpop; i++)
            {
                effect = putRectagle(oldpop[i]); // коэффициент эффективности 

                //Console.WriteLine($"Особь {i} КЕ {effect}");
                if (effect > qmax)
                {
                    qmax = effect;
                    p = i;
                }
                psred += effect;
            }
            sredmass[0] = psred / (double)numpop;
            maxmass[0] = qmax;
            bestEffect = qmax;
            BestOsob = oldpop[p];
            Console.WriteLine(BestOsob.arealPP);




        }

        public void crossover (RectLocation par1, RectLocation par2)
        {
            //Console.WriteLine("Родитель 1");
            
            //    Console.WriteLine($"{par1.rects[0].number},{par1.rects[1].number},{par1.rects[2].number},{par1.rects[3].number},{par1.rects[4].number},{par1.rects[5].number},{par1.rects[6].number},{par1.rects[7].number},{par1.rects[8].number},{par1.rects[9].number},{par1.rects[10].number}");
            
            //Console.WriteLine("Родитель 2");
            //Console.WriteLine($"{par2.rects[0].number},{par2.rects[1].number},{par2.rects[2].number},{par2.rects[3].number},{par2.rects[4].number},{par2.rects[5].number},{par2.rects[6].number},{par2.rects[7].number},{par2.rects[8].number},{par2.rects[9].number},{par2.rects[10].number}");

            RectLocation child1 = new RectLocation();
            for (int i = 0; i < ncub; i++)
            {
                RectagleC n = new RectagleC();

                child1.rects.Add(n);
            }
            RectLocation child2 = new RectLocation();
            for (int i = 0; i < ncub; i++)
            {
                RectagleC n = new RectagleC();
                child2.rects.Add(n);
            }
        List<RectagleC> listR = new List<RectagleC>();
            Random rnd = new Random();
            int p1,p2;
            double pcross;
            double pmut1;
            double pmut2;
            pcross = rnd.NextDouble();
            int sredncub = 0; 
            if (pcross <= numCrossof)
            {
                
                    // выставляем точки кроссовера
                    p1 = rnd.Next(1,ncub-3);
                    p2 = rnd.Next(p1+1,ncub-2);

                    // создаем 1 потомка
             
                    // заполняем центральные промежуток
                    for (int i=p1; i<= p2; i++)
                    {
                        
                        child1.rects[i] = par1.rects[i];
                        child1.rects[i].leftUpPoint = null;
                        child1.rects[i].rightUpPoint = null;
                        child1.rects[i].leftDownPoint = null;
                        child1.rects[i].rightDownPoint = null;                
                    }

                    bool inCentr = false;
                    // проверяем  оставшиеся элементы второго родителя  на наличие в центре первого потомка
                    for (int i=p2+1; i < ncub; i++) // проверяем конечный отрезок
                    {
                        for (int j=p1; j <= p2; j++)
                        {
                            if (par2.rects[i].number == child1.rects[j].number)
                            {
                                inCentr = true;
                            }
                        }
                        if (!inCentr)
                        {
                            listR.Add(par2.rects[i]);
                        }
                        inCentr=false;
                    }
                    for (int i = 0; i <=p2; i++) // проверяем начальный и центральный отрезок
                    {
                        for (int j = p1; j <= p2; j++)
                        {
                            if (par2.rects[i].number == child1.rects[j].number)
                            {
                                inCentr = true;
                            }
                        }
                        if (!inCentr)
                        {
                            listR.Add(par2.rects[i]);
                        }
                        inCentr = false;
                    }
                    for (int i = p2 + 1; i < ncub; i++) // заполняем конечный отрезок потомка
                    {
                        child1.rects[i] = listR[0];
                        child1.rects[i].leftUpPoint = null;
                        child1.rects[i].rightUpPoint = null;
                        child1.rects[i].leftDownPoint = null;
                        child1.rects[i].rightDownPoint = null;
                        listR.RemoveAt(0);
                    }
                    for (int i = 0; i <= p1-1; i++) // проверяем начальный отрезок
                    {
                        child1.rects[i] = listR[0];
                        child1.rects[i].leftUpPoint = null;
                        child1.rects[i].rightUpPoint = null;
                        child1.rects[i].leftDownPoint = null;
                        child1.rects[i].rightDownPoint = null;
                        listR.RemoveAt(0);
                    }

                    // Создаем 2 потомка

                    // заполняем центральные промежуток
                    for (int i = p1; i <= p2; i++)
                    {
                        child2.rects[i] = par2.rects[i];
                        child2.rects[i].leftUpPoint = null;
                        child2.rects[i].rightUpPoint = null;
                        child2.rects[i].leftDownPoint = null;
                        child2.rects[i].rightDownPoint = null;
                    }

                    // проверяем  оставшиеся элементы второго родителя  на наличие в центре первого потомка
                    for (int i = p2 + 1; i < ncub; i++) // проверяем конечный отрезок
                    {
                        for (int j = p1; j <= p2; j++)
                        {
                            if (par1.rects[i].number == child2.rects[j].number)
                            {
                                inCentr = true;
                            }
                        }
                        if (!inCentr)
                        {
                            listR.Add(par1.rects[i]);
                        }
                        inCentr = false;
                    }
                    for (int i = 0; i <= p2; i++) // проверяем начальный и центральный отрезок
                    {
                        for (int j = p1; j <= p2; j++)
                        {
                            if (par1.rects[i].number == child2.rects[j].number)
                            {
                                inCentr = true;
                            }
                        }
                        if (!inCentr)
                        {
                            listR.Add(par1.rects[i]);
                        }
                        inCentr = false;
                    }
                    for (int i = p2 + 1; i < ncub; i++) // заполняем конечный отрезок потомка
                    {
                        child2.rects[i] = listR[0];
                        listR.RemoveAt(0);
                        child2.rects[i].leftUpPoint = null;
                        child2.rects[i].rightUpPoint = null;
                        child2.rects[i].leftDownPoint = null;
                        child2.rects[i].rightDownPoint = null;
                    }
                    for (int i = 0; i <= p1 - 1; i++) // проверяем начальный и центральный отрезок
                    {
                        child2.rects[i] = listR[0];
                        listR.RemoveAt(0);
                        child2.rects[i].leftUpPoint = null;
                        child2.rects[i].rightUpPoint = null;
                        child2.rects[i].leftDownPoint = null;
                        child2.rects[i].rightDownPoint = null;
                    }
                pmut1 = rnd.NextDouble();
                if (pmut1 >= pmutation)
                {
                    child1 = mutation(child1);                    
                }
                pmut2 = rnd.NextDouble();
                if (pmut2 >= pmutation)
                {
                    child2 = mutation(child2);
                }

                RectLocation q1 = new RectLocation();
                
                for (int i=0; i < ncub; i++)
                {
                    RectagleC c = new RectagleC(child1.rects[i].w, child1.rects[i].h, child1.rects[i].number);
                    q1.rects.Add(c);
                }

                RectLocation q2 = new RectLocation();

                for (int i = 0; i < ncub; i++)
                {
                    RectagleC c = new RectagleC(child2.rects[i].w, child2.rects[i].h, child2.rects[i].number);
                    q2.rects.Add(c);
                }
                newpop.Add(q1);
                newpop.Add(q2);
                //Console.WriteLine("Ребенок 1");
                //Console.WriteLine($"{child1.rects[0].number},{child1.rects[1].number},{child1.rects[2].number},{child1.rects[3].number},{child1.rects[4].number},{child1.rects[5].number},{child1.rects[6].number},{child1.rects[7].number},{child1.rects[8].number},{child1.rects[9].number},{child1.rects[10].number}");
                //Console.WriteLine("Ребенок 2");
                //Console.WriteLine($"{child2.rects[0].number},{child2.rects[1].number},{child2.rects[2].number},{child2.rects[3].number},{child2.rects[4].number},{child2.rects[5].number},{child2.rects[6].number},{child2.rects[7].number},{child2.rects[8].number},{child2.rects[9].number},{child2.rects[10].number}");

            }

        }

        public RectLocation mutation (RectLocation osob)
        {
            Random rnd = new Random();
            int p = rnd.Next(0, ncub - 2);
            RectagleC m = new RectagleC();
            m = osob.rects[p];
            osob.rects[p]= osob.rects[p+1];
            osob.rects[p+1]= m;
            return (osob);
        }

        public void select() // сортируем старое поколение
        {
            Random rnd = new Random();
            List<int> n = new List<int>();
            int p1;
            int p2;
            RectLocation Osob1= new RectLocation();
            RectLocation Osob2= new RectLocation();
            for (int i = 0; i < numpop; i++)
            {
                n.Add(i);
            }

            for (int i = 0; i < numpop; i++)
            {
                p1 = rnd.Next(0, n.Count-1); // выбрали случайное число
                Osob1 = oldpop[n[p1]]; // выбрали 1 особь под этим номером                    
                n.RemoveAt(p1); // удалили номер особи из вариантов 
                p2 = rnd.Next(0, n.Count-1); // выбрали случайное число из оставшихся вариантов
                Osob2 = oldpop[n[p2]]; // выбрали 2 особь под этим номером
                if (!((Osob1.lucky) && (Osob2.lucky)))
                {
                    i--;
                }
                else
                {
                    if (Osob1.arealPP <= Osob2.arealPP)
                    {
                        intpop.Add(Osob1);
                    }
                    else
                    {
                        intpop.Add(Osob2);
                   
                    }
                    n.Clear();
                    for (int k = 0; k < numpop; k++)
                    {
                        n.Add(k);
                    }
                }
                
            }
        } 

        public void PaintChartRectagle(RectLocation pop)
        {
            double h;
            int h1;
            h = pictureBox1.Height / kHeight; // коэффициент маштаба по высоте
            h1 = (int)h; // целочисленный коэффициент маштаба по высоте

            pictureBox1.Width = (int)kWidth * h1;
            pictureBox1.Height = (int)kHeight * h1;

            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);

            Random rnd = new Random();
            int n;
            for (int i = 0; i < ncub; i++)
            {
                n = rnd.Next(0, colors.Count);
                SolidBrush myCollor = new SolidBrush(colors[n]);
                g.FillRectangle(myCollor, pop.rects[i].leftUpPoint.x * h1, pop.rects[i].leftUpPoint.y * h1, pop.rects[i].w * h1, pop.rects[i].h * h1);
                colors.RemoveAt(n);
            }
            Pen myWind = new Pen(Color.Black, 1);

            for (int i = 1; i < kWidth; i++)
            {
                g.DrawLine(myWind, i * h1, 0, i * h1, kHeight * h1);
            }

            for (int i = 1; i < kHeight; i++)
            {
                g.DrawLine(myWind, 0, i * h1, kWidth * h1, i * h1);
            }
        }

    }
}
