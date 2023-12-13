using System.IO;

var maps = new List<List<string>>();
maps.Add(new List<string>());

foreach (var line in File.ReadLines("input")) {
    if(line.Length > 0) {
        maps.Last().Add(line);
    } else {
        maps.Add(new List<string>());
    }
}

int total=0;
foreach (var map in maps) {
    (int horiz, int verti) = FindReflect(map);

    //check for smudges
    for(int r=0; r<map.Count; r++) {
        for(int c=0; c<map[0].Length; c++) {
            var smap = new List<string>(map);
            char ch = smap[r][c] == '#' ? '.' : '#';
            smap[r] = smap[r][..c] + ch + smap[r][(c+1)..];

            (int newh, int newv) = FindReflect(smap,horiz,verti);
            if (newh != 0) {
                horiz=newh;
                verti=0;
                goto gotsmudge;
            } else if(newv != 0) {
                horiz=0;
                verti=newv;
                goto gotsmudge;
            }
        }
    }

    gotsmudge:

    total+=verti+100*horiz;
}

Console.WriteLine(total);

(int horiz, int verti) FindReflect(List<string> map, int noth=0, int notv=0) {
   // horizontal reflection
    int horiz=0;
    for(int refl=1; refl<map.Count; refl++) {
        bool found=true;
        for(int r=0; refl-1-r>=0 && refl+r<map.Count; r++) {
            if(map[refl-1-r]!=map[refl+r]) {
                found=false;
                break;
            }
        }
        if(found && refl!=noth) {
            horiz=refl;
            break;
        }
    }

    // vertical reflection
    int verti=0;
    for(int refl=1; refl<map[0].Length; refl++) {
        bool found=true;
        for(int r=0; refl-1-r>=0 && refl+r<map[0].Length; r++) {
            if(!CompareCols(map,refl-1-r,refl+r)) {
                found=false;
                break;
            }
        }
        if(found && refl!=notv) {
            verti=refl;
            break;
        }
    }

    return (horiz,verti);
}

bool CompareCols(List<string> map, int c1, int c2) {
    foreach(var s in map) {
        if(s[c1] != s[c2]) return false;
    }
    return true;
}
