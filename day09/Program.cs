var seqs = new List<List<List<int>>>();

foreach(var line in File.ReadLines("input")) {
    var l = new List<int>(line.Split(' ').Select(s => int.Parse(s)));
    var s = new List<List<int>>();
    s.Add(l);
    seqs.Add(s);
}

int accum = 0;
int accum2 = 0;

foreach(var seq in seqs) {
    bool allzeros = false;
    while(!allzeros) {
        var der = new List<int>();
        int last = 0;
        bool first=true;
        allzeros = true;
        foreach(var n in seq.Last()) {
            if (!first) {
                der.Add(n-last);
                if(last-n != 0) allzeros = false;
            }
            first = false;
            last = n;
        }
        if (!allzeros) seq.Add(der);
    }

    var de = seq[^1][0]; // last derivatives are all the same
    var ds = de;
    for(int n = seq.Count-2; n >= 0; n--) {
        de += seq[n].Last();
        ds = seq[n][0] - ds;
    }

    accum += de;
    accum2 += ds;
}

Console.WriteLine(accum);
Console.WriteLine(accum2);