var map = File.ReadAllLines("input");

int h = map.Length;
int w = map[0].Length;

var energ = new bool[h,w];
int count;

var beamqueue = new Queue<(int r, int c, int dr, int dc)>();
var oldbeams = new HashSet<(int r, int c, int dr, int dc)>();

CountEnergy(0,0,0,1);
Console.WriteLine($"Part 1: {count}");

//brut
int bestcount=0;
for(int r=0; r<h; r++) {
    CountEnergy(r,0,0,1);
    if(count>bestcount) bestcount=count;
    CountEnergy(r,w-1,0,-1);
    if(count>bestcount) bestcount=count;
}
for(int c=0; c<w; c++) {
    CountEnergy(0,c,1,0);
    if(count>bestcount) bestcount=count;
    CountEnergy(h-1,c,-1,0);
    if(count>bestcount) bestcount=count;
}

Console.WriteLine($"Part 2: {bestcount}");


void CountEnergy(int sr, int sc, int sdr, int sdc) {
    //Console.Write($"{sr} {sc} {sdr} {sdc}: ");
    count = 0;
    oldbeams.Clear();
    for(int r=0; r<h; r++) {
        for(int c=0; c<w; c++) {
            energ[r,c] = false;
        }
    }
    AddBeam(sr,sc,sdr,sdc);
    while(beamqueue.Count > 0) {
        (int r, int c, int dr, int dc) = beamqueue.Dequeue();
        ShootBeam(r,c,dr,dc);
    }
    //Console.WriteLine(count);
}


void Energize(int r,int c) {
    if(!energ[r,c]) {
        energ[r,c]=true;
        count++;
    }
}

void AddBeam(int r, int c, int dr, int dc) {
    if(oldbeams.Add((r,c,dr,dc))) {
        beamqueue.Enqueue((r,c,dr,dc));
    }
}

void ShootBeam(int r, int c, int dr, int dc) {
    while(r>=0 && r<h && c>=0 && c<w && map[r][c] == '.') {
        Energize(r,c);
        r+=dr;
        c+=dc;
    }

    if(r>=0 && r<w && c>=0 && c<h) {
        Energize(r,c);
        switch(map[r][c]) {
            case '-':
                if(dr==0) AddBeam(r,c+dc,dr,dc);
                else {
                    AddBeam(r,c+1,0,1);
                    AddBeam(r,c-1,0,-1);
                }
                return;

            case '|':
                if(dc==0) AddBeam(r+dr,c,dr,dc);
                else {
                    AddBeam(r+1,c,1,0);
                    AddBeam(r-1,c,-1,0);
                }
                return;

            case '\\':
                AddBeam(r+dc,c+dr,dc,dr);
                return;

            case '/':
                AddBeam(r-dc,c-dr,-dc,-dr);
                return;
        }
    }
}