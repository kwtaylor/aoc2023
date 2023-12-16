using System.Xml;

string[] seqs;

using(var f = new StreamReader("input")) {
    seqs = f.ReadLine().Split(',');
}

int total=0;

foreach(var seq in seqs) {
    total+=HashOf(seq);
}

Console.WriteLine($"Part 1: {total}");

var boxes = new List<(string l,int f)>[256];

for (int b=0; b<256; b++) {
    boxes[b] = new List<(string l, int f)>();
}

foreach(var seq in seqs) {
    var idx = seq.IndexOfAny(['=','-']);
    var lab = seq[..idx];
    var box = HashOf(lab);
    var lens = boxes[box].FindIndex(a=>a.l==lab);
    if(seq[idx]=='=') {
        var foc = int.Parse(seq[(idx+1)..]);
        if(lens<0) {
            boxes[box].Add((lab,foc));
        } else {
            boxes[box][lens] = (lab,foc);
        }
    } else if (seq[idx]=='-') {
        if(lens>=0) {
            boxes[box].RemoveAt(lens);
        }
    }
}

int power=0;

for (int b=1; b<=256; b++) {
    int s=1;
    foreach(var lens in boxes[b-1]) {
        power+=b*s*lens.f;
        s++;
    }
}

Console.WriteLine($"Part 2: {power}");

static int HashOf(string s) {
    int hash=0;
    foreach(var c in s) {
        hash=Hash(c,hash);
    }
    return hash;
}

static int Hash(char c, int last) {
    last += c;
    last *= 17;
    last %= 256;
    return last;
}
