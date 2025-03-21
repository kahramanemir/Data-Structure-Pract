using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proje_3
{
    public class UM_Alani
    {
        public string AlanAdi;
        public List<string> ilAdlari;
        public int ilanYili;
        public List<string> ilBilgi;
        public UM_Alani(string AlanAdi, List<string> ilAdlari, int ilanYili, List<string> ilBilgi)
        {
            this.AlanAdi = AlanAdi;
            this.ilAdlari = ilAdlari;
            this.ilanYili = ilanYili;
            this.ilBilgi = ilBilgi;
        }
        public string ilAdlarıDondur()
        {
            string s = "";
            for (int i = 0; i < ilAdlari.Count; i++)
            {
                s += " " + ilAdlari[i];
            }
            return s;
        }
        private string ilParagrafBilgi()
        {
            string s = "";
            foreach(string bilgi in ilBilgi)
            {
                s += bilgi + " ";
            }
            return s;
        }
        public string toString()
        {
            return $"{AlanAdi} -- Şehir: {ilAdlarıDondur()} -- Yıl: {ilanYili} \n" +
                $"{ilParagrafBilgi()}\n";
        }
    }
    public class Node
    {
        public UM_Alani data;
        public Node leftNode, rightNode;
        public Node(UM_Alani data)
        {
            this.data = data;
            leftNode = null;
            rightNode = null;
        }
    }
    public class BinarySearchTree
    {
        private Node root;
        public BinarySearchTree()
        {
            root = null;
        }
        public void Ekle(UM_Alani data)
        {
            root = EkleRec(root, data);
        }
        private Node EkleRec(Node root, UM_Alani data)
        {
            if (root == null)
            {
                root = new Node(data);
                return root;
            }

            if (data.AlanAdi.CompareTo(root.data.AlanAdi) < 0)
            {
                root.leftNode = EkleRec(root.leftNode, data);
            }else if (data.AlanAdi.CompareTo(root.data.AlanAdi) > 0)
            {
                root.rightNode = EkleRec(root.rightNode, data);
            }
            return root;
        }
        public void InOrderTraversalYazdir()
        {
            InOrderTraversalYazdir(root);
        }
        private void InOrderTraversalYazdir(Node node)
        {
            if (node != null)
            {
                InOrderTraversalYazdir(node.leftNode);
                Console.WriteLine(node.data.AlanAdi);
                InOrderTraversalYazdir(node.rightNode);
            }
        }
        public void AgacBilgi()
        {
            int derinlik = Derinlik(root);
            int nodeSay = NodeSay(root);
            int dengeliDerinlik = DengeliDerinlikHesapla(nodeSay);

            Console.WriteLine("Ağaç Bilgiler:\n");
            Console.WriteLine($"Toplam düğüm sayısı: {nodeSay}");
            Console.WriteLine($"Ağacın derinliği: {derinlik}");
            Console.WriteLine($"Beklenen dengeli ağaç derinliği: {dengeliDerinlik}\n");
            Console.WriteLine();
            BilgileriListele(root);
        }
        private int Derinlik(Node node)
        {
            if (node == null)
                return 0;

            int solDerinlik = Derinlik(node.leftNode);
            int sagDerinlik = Derinlik(node.rightNode);

            return Math.Max(solDerinlik, sagDerinlik) + 1;
        }
        private int NodeSay(Node node)
        {
            if (node == null)
                return 0;

            return NodeSay(node.leftNode) + NodeSay(node.rightNode) + 1;
        }
        private int DengeliDerinlikHesapla(int nodeCount)
        {
            return (int)Math.Ceiling(Math.Log(nodeCount + 1, 2));
        }
        private void BilgileriListele(Node node)
        {
            if (node != null)
            {
                Console.WriteLine(node.data.toString());
                BilgileriListele(node.leftNode);
                BilgileriListele(node.rightNode);
            }
        }
        public void BalancedTree()
        {
            List<UM_Alani> siraliListe = new List<UM_Alani>();
            InOrderTraversal(root, siraliListe);
            root = BalancedTreeOlustur(siraliListe, 0, siraliListe.Count - 1);
            int derinlik = Derinlik(root);
            int nodeSay = NodeSay(root);
            int dengeliDerinlik = DengeliDerinlikHesapla(nodeSay);
            Console.WriteLine("Dengeli Ağaç Bilgileri:\n");
            Console.WriteLine($"Toplam düğüm sayısı: {nodeSay}");
            Console.WriteLine($"Beklenen dengeli ağaç derinliği: {dengeliDerinlik}");
            Console.WriteLine($"Ağacın derinliği: {derinlik}");
            Console.WriteLine();
            InOrderTraversalYazdir(root);
        }
        private void InOrderTraversal(Node node, List<UM_Alani> siraliListe)
        {
            if (node != null)
            {
                InOrderTraversal(node.leftNode, siraliListe);
                siraliListe.Add(node.data);
                InOrderTraversal(node.rightNode, siraliListe);
            }
        }
        private Node BalancedTreeOlustur(List<UM_Alani> siraliListe, int start, int end)
        {
            if (start > end)
            {
                return null;
            }

            int mid = (start + end) / 2;

            Node newNode = new Node(siraliListe[mid]);
            newNode.leftNode = BalancedTreeOlustur(siraliListe, start, mid - 1);
            newNode.rightNode = BalancedTreeOlustur(siraliListe, mid + 1, end);

            return newNode;
        }
        public void IkiHarfArasiListele(char ilkHarf, char ikinciHarf)
        {
            IkiHarfArasiListele(root, ilkHarf, ikinciHarf);
        }
        private void IkiHarfArasiListele(Node node, char ilkHarf, char sonHarf)
        {
            if (node != null)
            {
                char nodeHarf = Char.ToLower(node.data.AlanAdi[0]);
                int nodeHarfIndex = getIndex(nodeHarf);
                int ilkHarfIndex = getIndex(ilkHarf);
                int sonHarfIndex = getIndex(sonHarf);

                if (nodeHarfIndex >= ilkHarfIndex && nodeHarfIndex <= sonHarfIndex)
                {
                    Console.WriteLine(node.data.AlanAdi);
                }
                if (nodeHarfIndex < sonHarfIndex)
                {
                    IkiHarfArasiListele(node.rightNode, ilkHarf, sonHarf);
                }
                if (nodeHarfIndex > ilkHarfIndex)
                {
                    IkiHarfArasiListele(node.leftNode, ilkHarf, sonHarf);
                }
            }
        }
        private int getIndex(char harf)
        {
            char[] kucukAlfabe = { 'a', 'b', 'c', 'ç', 'd', 'e', 'f', 'g', 'ğ', 'h', 'ı', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'ö', 'p', 'r', 's', 'ş', 't', 'u', 'ü', 'v', 'w', 'x', 'y', 'z' };
            char[] buyukAlfabe = { 'A', 'B', 'C', 'Ç', 'D', 'E', 'F', 'G', 'Ğ', 'H', 'I', 'İ', 'J', 'K', 'L', 'M', 'N', 'O', 'Ö', 'P', 'R', 'S', 'Ş', 'T', 'U', 'Ü', 'V', 'W', 'X', 'Y', 'Z' };

            for (int i = 0; i < kucukAlfabe.Length; i++)
            {
                if (harf == buyukAlfabe[i]) { return i; }
                if (harf == kucukAlfabe[i]) { return i; }
            }
            return -1;
        }
    }
    public class HashTable
    {
        private Hashtable hashtable;

        public HashTable()
        {
            hashtable = new Hashtable();
        }

        public void UMAlaniEkle(UM_Alani alan)
        {
            hashtable.Add(alan.AlanAdi, alan);
        }

        public void AdıDegistir(string eskiAd, string yeniAd)
        {
            if(VarMi(eskiAd))
            {
                UM_Alani alan = (UM_Alani)hashtable[eskiAd];
                hashtable.Remove(eskiAd);
                alan.AlanAdi = yeniAd;
                hashtable.Add(yeniAd, alan);
                Console.WriteLine($"Ad güncellendi {eskiAd} --> {yeniAd}");
            }
            else
            {
                Console.WriteLine($"{eskiAd} Adında bir alan bulunamadı, tekrar deneyin.");
            }
        }
        public bool VarMi(string eskiAd)
        {
            foreach(DictionaryEntry entry in hashtable)
            {
                UM_Alani alan = (UM_Alani)entry.Value;
                if (eskiAd.ToLower() == alan.AlanAdi.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        public void HashTableYazdir()
        {
            foreach (DictionaryEntry entry in hashtable)
            {
                UM_Alani alan = (UM_Alani)entry.Value;
                Console.WriteLine($"Key: {entry.Key}\nValue: {alan.toString()}");
            }
        }
    }
    public class MinHeap
    {
        private List<UM_Alani> heapList;
        public MinHeap()
        {
            heapList = new List<UM_Alani>();
        }
        public void Ekle(UM_Alani eleman)
        {
            heapList.Add(eleman);
            trickleUp(heapList.Count - 1);
        }
        private void trickleUp(int index)
        {
            int parentIndex = (index - 1) / 2;

            while (index > 0 && string.Compare(heapList[index].AlanAdi, heapList[parentIndex].AlanAdi) < 0)
            {
                Swap(index, parentIndex);
                index = parentIndex;
                parentIndex = (index - 1) / 2;
            }
        }
        private void Swap(int index1, int index2)
        {
            UM_Alani temp = heapList[index1];
            heapList[index1] = heapList[index2];
            heapList[index2] = temp;
        }
        public UM_Alani HeapCikar()
        {
            UM_Alani minElement = heapList[0];
            int lastElementIndex = heapList.Count - 1;

            heapList[0] = heapList[lastElementIndex];
            heapList.RemoveAt(lastElementIndex);

            TrickleDown(0);

            return minElement;
        }
        private void TrickleDown(int index)
        {
            int leftChildIndex, rightChildIndex, minIndex;

            while (true)
            {
                leftChildIndex = 2 * index + 1;
                rightChildIndex = 2 * index + 2;
                minIndex = index;

                if (leftChildIndex < heapList.Count && string.Compare(heapList[leftChildIndex].AlanAdi, heapList[minIndex].AlanAdi) < 0)
                {
                    minIndex = leftChildIndex;
                }

                if (rightChildIndex < heapList.Count && string.Compare(heapList[rightChildIndex].AlanAdi, heapList[minIndex].AlanAdi) < 0)
                {
                    minIndex = rightChildIndex;
                }

                if (minIndex != index)
                {
                    Swap(index, minIndex);
                    index = minIndex;
                }
                else
                {
                    break;
                }
            }
        }
        public void HeapYazdir()
        {
            foreach (var eleman in heapList)
            {
                Console.WriteLine(eleman.toString());
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<UM_Alani> umAlaniBolgeleri = new List<UM_Alani>();
            string[] bolgeler = { "Akdeniz", "Doğu Anadolu", "Ege", "Güneydoğu Anadolu", "İç Anadolu", "Karadeniz", "Marmara" };
            string filePath1 = @"C:\repos\umAlanlari.txt";
            string filePath2 = @"C:\repos\umAlanlariParagraf.txt";
            BinarySearchTree tree = new BinarySearchTree();
            umAlaniBolgeleri = bolgeleriDoldur(umAlaniBolgeleri, filePath1, filePath2);
            foreach(UM_Alani alan in umAlaniBolgeleri)
            {
                tree.Ekle(alan);
            }
            HashTable hashTable = new HashTable();
            foreach (UM_Alani alan in umAlaniBolgeleri)
            {
                hashTable.UMAlaniEkle(alan);
            }
            MinHeap heap = new MinHeap();
            foreach (UM_Alani alan in umAlaniBolgeleri)
            {
                heap.Ekle(alan);
            }
            menu(tree, umAlaniBolgeleri, hashTable, heap);
        }

        static void menu(BinarySearchTree tree, List<UM_Alani> umAlaniBolgeleri, HashTable hashTable, MinHeap heap)
        {
            int secim = 0;
            while (secim != 8)
            {
                Console.WriteLine("1) Ağacı, derinliği, düğüm sayısını ve dengeli ağaç beklenen derinliğini yazdır\n" +
                                  "2) Iki harf arasındaki alanları yazdır\n" +
                                  "3) Dengeli ağaç oluştur\n" +
                                  "4) Hash Table yazdır\n" +
                                  "5) Hash Table eleman güncelle\n" +
                                  "6) Heap Yapısını yazdır\n" +
                                  "7) Heap 3 adet eleman çıkar\n" +
                                  "8) Bubble Sort");
                Console.Write("Seçiminizi giriniz: ");
                secim = Int32.Parse(Console.ReadLine());
                Console.WriteLine();
                switch (secim)
                {
                    case 1:
                        tree.AgacBilgi();
                        break;
                    case 2:
                        Console.Write("İlk harfi giriniz: ");
                        char ilkHarf = Char.Parse(Console.ReadLine());
                        Console.Write("Son harfi giriniz: ");
                        char sonHarf = Char.Parse(Console.ReadLine());
                        tree.IkiHarfArasiListele(ilkHarf, sonHarf);
                        Console.WriteLine();
                        break;
                    case 3:
                        tree.BalancedTree();
                        Console.WriteLine();
                        break;
                    case 4:
                        hashTable.HashTableYazdir();
                        break;
                    case 5:
                        Console.Write("Güncellemek istediğiniz alanın adını giriniz: ");
                        string eskiAd = Console.ReadLine();
                        Console.Write("Yeni alan adını giriniz: ");
                        string yeniAd = Console.ReadLine();
                        hashTable.AdıDegistir(eskiAd, yeniAd);
                        Console.WriteLine();
                        break;
                    case 6:
                        heap.HeapYazdir();
                        break;
                    case 7:
                        Console.WriteLine(heap.HeapCikar().toString());
                        Console.WriteLine(heap.HeapCikar().toString());
                        Console.WriteLine(heap.HeapCikar().toString());
                        break;
                    case 8:
                        int[] array = {5, 8, 47, 16, 91, 61, 30, 3, 7, 38, 29, 54};
                        Console.WriteLine("Sıralanmamış Array: ");
                        printArray(array);
                        BubbleSort(array);
                        break;

                }
            }
        }
        static List<UM_Alani> bolgeleriDoldur(List<UM_Alani> umAlaniBolgeleri, string filePath1, string filePath2)
        {
            List<string> umAlanlariBilgi = new List<string>();
            List<string> umAlanlariParagrafList;
            umAlanlariBilgi = File.ReadAllLines(filePath1).ToList();
            umAlanlariParagrafList = File.ReadAllLines(filePath2).ToList();
            string umAlanlariParagraf = "";
            int sayac1 = 0;
            string[] umAlanlariParagrafArray = new string[umAlanlariParagrafList.Count];
            foreach (string paragraf in umAlanlariParagrafList)
            {
                umAlanlariParagrafArray[sayac1] = paragraf;
                sayac1++;
            }
            int sayac2 = 0;
            foreach (string umAlaniBilgi in umAlanlariBilgi)
            {
                List<string> umAlaniParagrafListesi = new List<string>();
                string Alan_adi;
                List<string> il_Adlari = new List<string>();
                int ilanYili;
                umAlanlariParagraf = umAlanlariParagrafArray[sayac2];
                umAlanlariParagrafList = paragrafiListeyeCevir(umAlanlariParagraf);
                if (sayac2 == 6)
                {
                    Alan_adi = "Xanthos-Letoon";
                    il_Adlari.Add("Antalya");
                    il_Adlari.Add("Muğla");
                    ilanYili = 1988;
                }
                else if (sayac2 == 20)
                {
                    string[] umAlaniBilgiArray = new string[5];
                    umAlaniBilgiArray = umAlaniBilgi.Split('/');
                    Alan_adi = umAlaniBilgiArray[0];
                    il_Adlari.Add("Konya");
                    il_Adlari.Add("Kastamonu");
                    il_Adlari.Add("Eskişehir");
                    il_Adlari.Add("Afyon");
                    il_Adlari.Add("Ankara");
                    ilanYili = Int32.Parse(umAlaniBilgiArray[2]);
                }
                else
                {
                    string[] umAlaniBilgiArray = new string[3];
                    umAlaniBilgiArray = umAlaniBilgi.Split('/');
                    Alan_adi = umAlaniBilgiArray[0];
                    il_Adlari.Add(umAlaniBilgiArray[1]);
                    ilanYili = Int32.Parse(umAlaniBilgiArray[2]);
                }
                UM_Alani umAlani = new UM_Alani(Alan_adi, il_Adlari, ilanYili, umAlanlariParagrafList);
                umAlaniBolgeleri.Add(umAlani);
                sayac2++;
            }
            return umAlaniBolgeleri;
        }
        static List<string> paragrafiListeyeCevir(string paragraf)
        {
            List<string> paragrafList = new List<string>();
            string[] paragrafArray = paragraf.Split(' ');
            foreach(string kelime in paragrafArray)
            {
                if (kelime != null)
                {
                    paragrafList.Add(kelime);
                }
            }
            return paragrafList;
        }
        static void printArray(int[] array)
        {
            foreach (int element in array)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine();
        }
        static void BubbleSort(int[] array)
        {
            int sayac = 0;
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        sayac++;
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                        Console.Write($"{sayac.ToString().PadLeft(2)}. Adım Array: ");
                        printArray(array);
                    }
                }
            }
        }
    }
}
