using System;
using System.Collections.Generic;
using System.Text;

namespace GameCenter {
    class MapRoom {

        public List<string>[] octanom = new List<string>[ 6];
        public List<string>[] sokoban = new List<string>[99];

        public MapRoom() {
            Load_Maps();
        }

        private void Load_Maps() {
            octanom[0] = new List<string>(); octanom[1] = new List<string>(); octanom[2] = new List<string>();
            octanom[0].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[1].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[2].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            octanom[0].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[1].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[2].Add("XXXOXXXXXXXXXXXXXXXXXXXXXTXX");
            octanom[0].Add("GCCCCCCCCCCCCXXCCCCCCCCCCCCG"); octanom[1].Add("XXXXXXXTXXXXXXXXXXXXXXXXXXXX"); octanom[2].Add("GCCCCCCCCCCCCCCCCCCCCCCCCCCG");
            octanom[0].Add("XPXXXXCXXXXXCXXCXXXXXCXXXXPX"); octanom[1].Add("GCCCCCCCCCCCCCCCCCCCCCCCCCCG"); octanom[2].Add("XCPCXXXXXXXXXXXXXXXXXXXXCPCX");
            octanom[0].Add("XCXXXXCXXXXXCXXCXXXXXCXXXXCX"); octanom[1].Add("XCXXXXXCXXCXXXXXXXXXXXCXXXCX"); octanom[2].Add("XCXCCCCCCCCCCCCCCCCCCCCCCXCX");
            octanom[0].Add("XCCCCCCCCCCCCCCCCCCCCCCCCCCX"); octanom[1].Add("XCCPXCCCXXCCCCCCCCCCCXPCCCCX"); octanom[2].Add("XCXCXCCCXXXXXXXXXXXXCCCXCXCX");
            octanom[0].Add("XCXXXXCXXCXXXXXXXXCXXCXXXXCX"); octanom[1].Add("XCXCCCXXXXXXCXCXXXXXCXCXCXCX"); octanom[2].Add("XCXCXCXCCCCCCCCCCCCCCXCXCXCX");
            octanom[0].Add("XCCCCCCXXCCCCXXCCCCXXCCCCCCX"); octanom[1].Add("XCXCXCCCXXCCCXCXCXCCCCCXCXCX"); octanom[2].Add("XCXCXCXCXCXCXXXXCXCXCXCXCXCX");
            octanom[0].Add("XXXXXXCXXXXXCXXCXXXXXCXXXXXX"); octanom[1].Add("XCCCXCXCXXCXCXCXCXCXCXXXCXCX"); octanom[2].Add("XCXCXCXCXCXCCCCCCXCXCXCXCXCX");
            octanom[0].Add("XXXXXXCXX   CCCC   XXCXXXXXX"); octanom[1].Add("XCXCXCXCXXCXCXCXCXCXCXCXCXCX"); octanom[2].Add("XCXCXCXCXCXCX  XCXCXCXCXCXCX");
            octanom[0].Add("XXXXXXC   XXXXXXXX   CXXXXXX"); octanom[1].Add("XCCCXCXCXXCXCCCXCXCXCXCXCXCX"); octanom[2].Add("XCXCXCXCXCXCX  XCXCXCXCXCXCX");
            octanom[0].Add("XXXXXXCXX XXXXXXXX XXCXXXXXX"); octanom[1].Add("XCXCXCXCCCCXCXCXCXCXCCCXCXCX"); octanom[2].Add("XCXCXCXCXCXCX  XCXCXCXCXCXCX");
            octanom[0].Add("XXXXXXCXX XOXXXXTX XXCXXXXXX"); octanom[1].Add("XCXCXCXXXXXXCXCCCXCXCXCXCXCX"); octanom[2].Add("XCXCXCXCXCXCX  XCXCXCXCXCXCX");
            octanom[0].Add("XXXXXXCXX          XXCXXXXXX"); octanom[1].Add("XCXCXCXCCCCXCXCXCXCXCXCCCCCX"); octanom[2].Add("XCXCXCXCXCXCX  XCXCXCXCXCXCX");
            octanom[0].Add("XCCCCCCXXXXX XX XXXXXCCCCCCX"); octanom[1].Add("XCXCXCXCXXCXCXCXCXCXCXCXCXCX"); octanom[2].Add("XCXCXCXCXCXCCCCCCXCXCXCXCXCX");
            octanom[0].Add("XCXXXXCXXXXX XX XXXXXCXXXXCX"); octanom[1].Add("XCXCXCXCXXCCCCCXCCCCCCCXCXCX"); octanom[2].Add("XCXCXCXCXCCCXXXXCCCXCXCXCXCX");
            octanom[0].Add("XCCPXXCCCCCCCCCCCCCCCCXXPCCX"); octanom[1].Add("XCXCXCCCXXXXXXXXCXCXCXCXCXCX"); octanom[2].Add("XCXCXCXCCCCCCCCCCCCCCXCXCXCX");
            octanom[0].Add("XXXCXXCXXCXXXXXXXXCXXCXXCXXX"); octanom[1].Add("XCXCCPXCXXCCCCCCCXCXCXCCCXCX"); octanom[2].Add("XCXCXCCCXXXXXXXXXXXXCXCXCXCX");
            octanom[0].Add("XCCCCCCXXCCCCXXCCCCXXCCCCCCX"); octanom[1].Add("XCXXCXXXXXCXXXXXCXCXCXCXXXCX"); octanom[2].Add("XCXCCCCCCCCCCCCCCCCCCCCCCXCX");
            octanom[0].Add("XCXXXXXXXXXXCXXCXXXXXXXXXXCX"); octanom[1].Add("GCCCCCCCCCCCCCCCPCCCCCCCCCCG"); octanom[2].Add("XCPCXXXXXXXXXXXXXXXXXXXXCPCX");
            octanom[0].Add("GCCCCCCCCCCCCCCCCCCCCCCCCCCG"); octanom[1].Add("XXXXXXXOXXXXXXXXXXXXXXXXXXXX"); octanom[2].Add("GCCCCCCCCCCCCCCCCCCCCCCCCCCG");
            octanom[0].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[1].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[2].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            octanom[0].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[1].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[2].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX");

            octanom[3] = new List<string>(); octanom[4] = new List<string>(); octanom[5] = new List<string>();
            octanom[3].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[4].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[5].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            octanom[3].Add("XXXXXXXXXTXXXXXXXOXXXXXXXXXX"); octanom[4].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[5].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            octanom[3].Add("GPCCCCCCCCCCCCCCCCCCCCCCCCPG"); octanom[4].Add("GCCCCCCCCCCCCCCCCCCCCCCCCCCG"); octanom[5].Add("GPCCCCCCCCCCCCCCCCCCCCCCCCPG");
            octanom[3].Add("XXCXXXXXXCXXXXXXXCXXXXXXXCXX"); octanom[4].Add("XCXXXXPXXXXXXXXXXXXXXPXXXXCX"); octanom[5].Add("XXCXXXXXXXCXXXXXXCXXXXXXXCXX");
            octanom[3].Add("XCCCCCCCCCCCCCCCCCCCCCCCCCCX"); octanom[4].Add("XCCCCCCCCCCCCXXCCCCCCCCCCCCX"); octanom[5].Add("XCCCXCCCXCCCXCCXCCCXCCCXCCCX");
            octanom[3].Add("XCXXCXXXXXXXXCXXXXXXXXXCXXCX"); octanom[4].Add("XCXXCXXXCXXXCXXCXXXCXXXCXXCX"); octanom[5].Add("XCXCXCXCXCXCXCCXCXCXCXCXCXCX");
            octanom[3].Add("XCXXCXXXXXXXXCXXXXXXXXXCXXCX"); octanom[4].Add("XCXXCXOXCXXXCXXCXXXCXTXCXXCX"); octanom[5].Add("XCXCXCXCXCXCXCCXCXCXCXCXCXCX");
            octanom[3].Add("XCXCCCCCCCCCCCCCCCCCCCCCCXCX"); octanom[4].Add("XCXCCCCCCCXXCXXCXXCCCCCCCXCX"); octanom[5].Add("XCXCCCXCCCXCXCCXCXCCCXCCCXCX");
            octanom[3].Add("XCCCXXCXXXXCXXXXCXXXXCXXCCCX"); octanom[4].Add("XCCCXXCXXCCCCXXCCCCXXCXXCCCX"); octanom[5].Add("XCXCXCXCXCXCXCCXCXCXCXCXCXCX");
            octanom[3].Add("XCXCXCCCCCCCCCCCCCCCCCCXCXCX"); octanom[4].Add("XCXCXCCCXCXXCXXCXXCXCCCXCXCX"); octanom[5].Add("XCXCXCXCXCXCXCCXCXCXCXCXCXCX");
            octanom[3].Add("XCXCXCXCXXXXXCXXXXXXCXCXCXCX"); octanom[4].Add("XCXCXCXCXCXXCXXCXXCXCXCXCXCX"); octanom[5].Add("XCXCXCXCXCXCXCCXCXCXCXCXCXCX");
            octanom[3].Add("XCXCCCXCCCCCCCCCCCCCCXCCCXCX"); octanom[4].Add("XCXCCCXCCCXXCCCCXXCCCXCCCXCX"); octanom[5].Add("XCXCXCXCXCXCCCCCCXCXCXCXCXCX");
            octanom[3].Add("XCXCXCXCXXXXXCXXXXXXCXCXCXCX"); octanom[4].Add("XCXCXCXCXCXXCXXCXXCXCXCXCXCX"); octanom[5].Add("XCXCXCXCXCXCXCCXCXCXCXCXCXCX");
            octanom[3].Add("XCXCXCXCXXXXXCXXXXXXCXCXCXCX"); octanom[4].Add("XCXCXCCCXCXXCXXCXXCXCCCXCXCX"); octanom[5].Add("XCXCXCXCXCXCXCCXCXCXCXCXCXCX");
            octanom[3].Add("XCXCXCCCCCCCCCCCCCCCCCCXCXCX"); octanom[4].Add("XCCCXXCXXCCCCXXCCCCXXCXXCCCX"); octanom[5].Add("XCXCXCXCXCXCXCCXCXCXCXCXCXCX");
            octanom[3].Add("XCCCXXCXXXXCXXXXCXXXXCXXCCCX"); octanom[4].Add("XCXCCCCCCCXXCXXCXXCCCCCCCXCX"); octanom[5].Add("XCCCXCCCXCCCXCCXCCCXCCCXCCCX");
            octanom[3].Add("XCXCCCCCCCCCCCCCCCCCCCCCCXCX"); octanom[4].Add("XCXXCXXXCXXXCXXCXXXCXXXCXXCX"); octanom[5].Add("XCXCXCXCXCXCXCCXCXCXCXCXCXCX");
            octanom[3].Add("XCXXCXXXXXXXXXXXXXXXXXXCXXCX"); octanom[4].Add("XCXXCXXXCXXXCXXCXXXCXXXCXXCX"); octanom[5].Add("XCXCXCXCXCXCXCCXCXCXCXCXCXCX");
            octanom[3].Add("XCCCCCCCCCCCCCCCCCCCCCCCCCCX"); octanom[4].Add("XCCCCCCCCCCCCXXCCCCCCCCCCCCX"); octanom[5].Add("XCXCCCXCCCXCXCCXCXCCCXCCCXCX");
            octanom[3].Add("XXCXXXXXXCXXXXXXXCXXXXXXXCXX"); octanom[4].Add("XCXXXXPXXXXXXXXXXXXXXPXXXXCX"); octanom[5].Add("XXXXCXOXCXXXXXXXXXXCXTXCXXXX");
            octanom[3].Add("GPCCCCCCCCCCCCCCCCCCCCCCCCPG"); octanom[4].Add("GCCCCCCCCCCCCCCCCCCCCCCCCCCG"); octanom[5].Add("GPCCCCCCCCCCCCCCCCCCCCCCCCPG");
            octanom[3].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[4].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[5].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            octanom[3].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[4].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX"); octanom[5].Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXX");



            sokoban[0] = new List<string>(); sokoban[1] = new List<string>(); sokoban[2] = new List<string>(); sokoban[3] = new List<string>(); sokoban[4] = new List<string>();
            sokoban[0].Add("                "); sokoban[1].Add("                "); sokoban[2].Add("                "); sokoban[3].Add("                "); sokoban[4].Add("      XXXX XXXX ");
            sokoban[0].Add("      XXXX      "); sokoban[1].Add("XXXX    XXX     "); sokoban[2].Add("XXXXXX  XXXXXXXX"); sokoban[3].Add("XXXXXXXXXXXXXXXX"); sokoban[4].Add("XXXXXXX''XXX''X ");
            sokoban[0].Add("    XXXO'XX     "); sokoban[1].Add("XM'XXX  XOX     "); sokoban[2].Add("XO'''XXXX''X'''X"); sokoban[3].Add("X''''''''''''''X"); sokoban[4].Add("X''MMMX'''''C'X ");
            sokoban[0].Add("    X'CC''X     "); sokoban[1].Add("XM'''XXXX'XXXX  "); sokoban[2].Add("XXX'C'''XC'''C'X"); sokoban[3].Add("X''C'C'C'C'C'C'X"); sokoban[4].Add("X'XMXXX''XXX''X ");
            sokoban[0].Add("    X''C''X     "); sokoban[1].Add("XM'''''XX'X''X  "); sokoban[2].Add("XMM''C''X''XXXXX"); sokoban[3].Add("X'X'XXXXXXXX'XXX"); sokoban[4].Add("X'M''C''XXXX'XX ");
            sokoban[0].Add("    X'''''X     "); sokoban[1].Add("XXX'X'''''''CX  "); sokoban[2].Add("XMM''X''XC'''MMX"); sokoban[3].Add("X'X''C'C'XMMMMMX"); sokoban[4].Add("XXX''O'''''X'X  ");
            sokoban[0].Add("    XXX''XX     "); sokoban[1].Add("  X'''X'XXXX'XX "); sokoban[2].Add("XMM''X''X''''MMX"); sokoban[3].Add("X'XOC'C'C'M'''MX"); sokoban[4].Add("  XX'XX''''X'X  ");
            sokoban[0].Add("      X'''X     "); sokoban[1].Add(" XX'X'X'''''''X "); sokoban[2].Add("XXX''XC''''X'XXX"); sokoban[3].Add("X'XC'C'C'XM'M'MX"); sokoban[4].Add("   X''XXXCXX'X  ");
            sokoban[0].Add("      XMMMX     "); sokoban[1].Add(" X'CX'X'X'C'''X "); sokoban[2].Add("X''''X'''C'''C'X"); sokoban[3].Add("X'''C'C'C'M'''MX"); sokoban[4].Add("   X'CXXX'XX'XX ");
            sokoban[0].Add("      XXXXX     "); sokoban[1].Add(" X''''''''''XXX "); sokoban[2].Add("X''''X''X''''''X"); sokoban[3].Add("X'X''''''XMMMMMX"); sokoban[4].Add("   X''''''C'''X ");
            sokoban[0].Add("                "); sokoban[1].Add(" XXXXXXXXXXXX   "); sokoban[2].Add("XXXXXXXXXXXXXXXX"); sokoban[3].Add("XXXXXXXXXXXXXXXX"); sokoban[4].Add("   XXXXXXXX'''X ");
            sokoban[0].Add("                "); sokoban[1].Add("                "); sokoban[2].Add("                "); sokoban[3].Add("                "); sokoban[4].Add("          XXXXX ");

            sokoban[5] = new List<string>(); sokoban[6] = new List<string>(); sokoban[7] = new List<string>(); sokoban[8] = new List<string>(); sokoban[9] = new List<string>();
            sokoban[5].Add("                "); sokoban[6].Add("    XXXXXX      "); sokoban[7].Add("                "); sokoban[8].Add("                "); sokoban[9].Add("                ");
            sokoban[5].Add("                "); sokoban[6].Add("    X'X''X      "); sokoban[7].Add("                "); sokoban[8].Add("  XXXXXXXXXXX   "); sokoban[9].Add("XXXXXX XXXXXXXX ");
            sokoban[5].Add("   XXXX         "); sokoban[6].Add("    XM'M'X      "); sokoban[7].Add(" XXXXXX         "); sokoban[8].Add("  XM''''''''X   "); sokoban[9].Add("XM''MXXX''''''XX");
            sokoban[5].Add("   X''XXXXXXX   "); sokoban[6].Add("  XXX'XC'XXXXXX "); sokoban[7].Add("XX''''XX   XXXX "); sokoban[8].Add("  X'X'XCXXX'X   "); sokoban[9].Add("XM''C'''''C''C'X");
            sokoban[5].Add("   X''''CMC'X   "); sokoban[6].Add("  X'MC'''C'X''X "); sokoban[7].Add("X'CXX''XXXXX''X "); sokoban[8].Add("  XCXCC''M''XX  "); sokoban[9].Add("XMXMC'C'C''CCX'X");
            sokoban[5].Add("   X''MX'M''X   "); sokoban[6].Add("XXX'CMCM'MC'''X "); sokoban[7].Add("X''''''MM'XXC'X "); sokoban[8].Add("  X'''X'X'''MX  "); sokoban[9].Add("XMXXXXXCXX''C''X");
            sokoban[5].Add("   X''MXCOCXX   "); sokoban[6].Add("XM''COMCMXXM''X "); sokoban[7].Add("X'CX'''MM'''''X "); sokoban[8].Add("  XM''C'X'XCXX  "); sokoban[9].Add("XMX''''C'''X'C'X");
            sokoban[5].Add("   XXX'''''X    "); sokoban[6].Add("XXCX'CXX''X'CXX "); sokoban[7].Add("X'''CXXXX''XXXX "); sokoban[8].Add("  XXX'X''''''X  "); sokoban[9].Add("XMX'XXXXXX'XX''X");
            sokoban[5].Add("     XXXXXXX    "); sokoban[6].Add(" X''X''XX'XX'X  "); sokoban[7].Add("XX'''X  XXOX    "); sokoban[8].Add("    X'X'X'X''X  "); sokoban[9].Add("XMX''''C'C'MXMMX");
            sokoban[5].Add("                "); sokoban[6].Add(" XX'''MX'C''MX  "); sokoban[7].Add(" XXXXX   XXX    "); sokoban[8].Add("    X'''XMO'MX  "); sokoban[9].Add("XXXXX''''''MXOMX");
            sokoban[5].Add("                "); sokoban[6].Add("  XXX''''''X'X  "); sokoban[7].Add("                "); sokoban[8].Add("    XXXXXXXXXX  "); sokoban[9].Add("    XXXXXXXXXXXX");
            sokoban[5].Add("                "); sokoban[6].Add("    XXXXXXXXXX  "); sokoban[7].Add("                "); sokoban[8].Add("                "); sokoban[9].Add("                ");

            sokoban[10] = new List<string>(); sokoban[11] = new List<string>(); sokoban[12] = new List<string>(); sokoban[13] = new List<string>(); sokoban[14] = new List<string>();
            sokoban[10].Add("                "); sokoban[11].Add("                "); sokoban[12].Add("                "); sokoban[13].Add("                "); sokoban[14].Add("                ");
            sokoban[10].Add("XXXXXXXXXXXXX   "); sokoban[11].Add("    XXXXXXXX    "); sokoban[12].Add("    XXXXXXXXX   "); sokoban[13].Add("                "); sokoban[14].Add(" XXXX XXXXXXXXX ");
            sokoban[10].Add("XM'MX'''''''XXXX"); sokoban[11].Add("    X''''''X    "); sokoban[12].Add("    X'''X''OX   "); sokoban[13].Add("   XXXXXXX      "); sokoban[14].Add(" X''XXX'''''''X ");
            sokoban[10].Add("XMM'XC'C'X'CX''X"); sokoban[11].Add("    X'XCXMXX    "); sokoban[12].Add("   XX''MCMX'X   "); sokoban[13].Add("   XMMM''XXX    "); sokoban[14].Add("XX'C'C''X'XXX'X ");
            sokoban[10].Add("X'M'XOCCCC''''MX"); sokoban[11].Add("    X'C'MCMX    "); sokoban[12].Add("   X''XXC'C'X   "); sokoban[13].Add("   XOXXCC''X    "); sokoban[14].Add("X'''C'XXXM'M''X ");
            sokoban[10].Add("XM'MXC''C'''XCMX"); sokoban[11].Add("    X'X'''OX    "); sokoban[12].Add("  XX''MCMXCXX   "); sokoban[13].Add("   X'C'''''X    "); sokoban[14].Add("X'XCC'XXM'M'X'X ");
            sokoban[10].Add("XX'XX''CCCCCX'MX"); sokoban[11].Add("    X'X'MCMX    "); sokoban[12].Add("  X''XCX'''X    "); sokoban[13].Add("   XMX'XX'XX    "); sokoban[14].Add("X''''CXM'M''X'X ");
            sokoban[10].Add("XX'X'C''''''XCMX"); sokoban[11].Add("    X'MC'''X    "); sokoban[12].Add("  X''M'MC'XX    "); sokoban[13].Add("   X'C'X''X     "); sokoban[14].Add("X'CX''C'MXM'''X ");
            sokoban[10].Add("XM'''C'XXXX'XMMX"); sokoban[11].Add("    X'XCXXXX    "); sokoban[12].Add("  XXXX''''X     "); sokoban[13].Add("   XXX'''XX     "); sokoban[14].Add("X''C''XM'''MXXX ");
            sokoban[10].Add("XM''X''XMMM'''XX"); sokoban[11].Add("    X''''''X    "); sokoban[12].Add("     XXXXXX     "); sokoban[13].Add("     XXXXX      "); sokoban[14].Add("X'CXXXXXX'OXX   ");
            sokoban[10].Add("XXXXXXXXXXXXXXX "); sokoban[11].Add("    XXXXXXXX    "); sokoban[12].Add("                "); sokoban[13].Add("                "); sokoban[14].Add("XXXX            ");
            sokoban[10].Add("                "); sokoban[11].Add("                "); sokoban[12].Add("                "); sokoban[13].Add("                "); sokoban[14].Add("                ");

            sokoban[15] = new List<string>(); sokoban[16] = new List<string>(); sokoban[17] = new List<string>(); sokoban[18] = new List<string>(); sokoban[19] = new List<string>();
            sokoban[15].Add("   XXXXX        "); sokoban[16].Add(" XXXX     XXXX  "); sokoban[17].Add("                "); sokoban[18].Add("                "); sokoban[19].Add("                ");
            sokoban[15].Add("   X'''XXXXX    "); sokoban[16].Add(" X''XXXXXXX''X  "); sokoban[17].Add("                "); sokoban[18].Add("                "); sokoban[19].Add("                ");
            sokoban[15].Add("   X'X'XX''X    "); sokoban[16].Add(" X'''''''C'''X  "); sokoban[17].Add(" XXXX      XXXX "); sokoban[18].Add("    XXXXXXX     "); sokoban[19].Add("    XXXXXXX     ");
            sokoban[15].Add("   X'C''''CXX   "); sokoban[16].Add(" X''X'X'XXX''X  "); sokoban[17].Add(" X''XXXXXXXX''X "); sokoban[18].Add("  XXX''M''XXXX  "); sokoban[19].Add("    X''X''XXX   ");
            sokoban[15].Add("   XXX'MMX''X   "); sokoban[16].Add(" XX'XCC'MMX'XX  "); sokoban[17].Add(" X'C'X'MM'X'C'X "); sokoban[18].Add(" XX'C'XMX'C''X  "); sokoban[19].Add("  XXXMCMCM'OX   ");
            sokoban[15].Add("     XCMMM''X   "); sokoban[16].Add("  X'COXMMMXCX   "); sokoban[17].Add(" X''CCCMMCCC''X "); sokoban[18].Add(" X'C'CXMXC'C'X  "); sokoban[19].Add("  X''M'XCMC'X   ");
            sokoban[15].Add("   XXX'OMMX'X   "); sokoban[16].Add("  X'XCXMMXX'XX  "); sokoban[17].Add(" XX'''CM''''''X "); sokoban[18].Add(" X''CMM'MMC''X  "); sokoban[19].Add("  X'XC'MCM'XX   ");
            sokoban[15].Add("   X'C''C'C'X   "); sokoban[16].Add("  X''C'XMC'''X  "); sokoban[17].Add("  XO'XMMMMX''XX "); sokoban[18].Add(" XXX'XXOXX'XXX  "); sokoban[19].Add("  X''M'CC'XX    ");
            sokoban[15].Add("   X'X'XXXC'X   "); sokoban[16].Add("  XX''C'MX'''X  "); sokoban[17].Add("  XXXXXXXXXXXX  "); sokoban[18].Add("   X'''M'''X    "); sokoban[19].Add("  XXX''X''X     ");
            sokoban[15].Add("   X'''X X''X   "); sokoban[16].Add("   XX''XXXXXXX  "); sokoban[17].Add("                "); sokoban[18].Add("   XXXXXXXXX    "); sokoban[19].Add("    XXXXXXX     ");
            sokoban[15].Add("   XXXXX XXXX   "); sokoban[16].Add("    X''X        "); sokoban[17].Add("                "); sokoban[18].Add("                "); sokoban[19].Add("                ");
            sokoban[15].Add("                "); sokoban[16].Add("    XXXX        "); sokoban[17].Add("                "); sokoban[18].Add("                "); sokoban[19].Add("                ");

            sokoban[20] = new List<string>(); sokoban[21] = new List<string>(); sokoban[22] = new List<string>(); sokoban[23] = new List<string>(); sokoban[24] = new List<string>();
            sokoban[20].Add("                "); sokoban[21].Add("                "); sokoban[22].Add("                "); sokoban[23].Add("                "); sokoban[24].Add("                ");
            sokoban[20].Add("                "); sokoban[21].Add("     XXXX       "); sokoban[22].Add("      XXXX      "); sokoban[23].Add("   XXXXXXXXX    "); sokoban[24].Add("      XXXXX     ");
            sokoban[20].Add("     XXXXXX     "); sokoban[21].Add("   XXX''XXXX    "); sokoban[22].Add("    XXX''X      "); sokoban[23].Add("   X'''XX''X    "); sokoban[24].Add("     XX'''XXX   ");
            sokoban[20].Add("   XXX'OM'X     "); sokoban[21].Add("   X''C''C'X    "); sokoban[22].Add("   XX''''XXX    "); sokoban[23].Add("   X'''''''X    "); sokoban[24].Add("    XX''''''X   ");
            sokoban[20].Add("   X'M'CX'XX    "); sokoban[21].Add("   X'CXMXC'X    "); sokoban[22].Add("   X'C'C'''X    "); sokoban[23].Add("   XXCXMXCCXX   "); sokoban[24].Add("   XX'CXCCX'XX  ");
            sokoban[20].Add("   X'C'M'C'X    "); sokoban[21].Add("   X'MMMMMOX    "); sokoban[22].Add("   X'XCX'XCXX   "); sokoban[23].Add("   XMMMCMMMMX   "); sokoban[24].Add("  XX'C''C''C'X  ");
            sokoban[20].Add("   XX'X'CM'X    "); sokoban[21].Add("   XX'XMXCXX    "); sokoban[22].Add("   X'M'MCM''X   "); sokoban[23].Add("  XX'XCXCX''X   "); sokoban[24].Add("  X''X'XCX'X'X  ");
            sokoban[20].Add("    X'MC'XXX    "); sokoban[21].Add("   X''C''C'X    "); sokoban[22].Add("   XXMOM'XC'X   "); sokoban[23].Add("  X''C'''C'XX   "); sokoban[24].Add("  X''MMMOMMMMX  ");
            sokoban[20].Add("    XX''XX      "); sokoban[21].Add("   X'''X'''X    "); sokoban[22].Add("    XXXM'''XX   "); sokoban[23].Add("  X'''XO''XX    "); sokoban[24].Add("  X''XXXXXXXXX  ");
            sokoban[20].Add("     XXXX       "); sokoban[21].Add("   XXXXXXXXX    "); sokoban[22].Add("      X''XXX    "); sokoban[23].Add("  XXXXXXXXX     "); sokoban[24].Add("  XXXX          ");
            sokoban[20].Add("                "); sokoban[21].Add("                "); sokoban[22].Add("      XXXX      "); sokoban[23].Add("                "); sokoban[24].Add("                ");
            sokoban[20].Add("                "); sokoban[21].Add("                "); sokoban[22].Add("                "); sokoban[23].Add("                "); sokoban[24].Add("                ");

            sokoban[25] = new List<string>(); sokoban[26] = new List<string>(); sokoban[27] = new List<string>(); sokoban[28] = new List<string>(); sokoban[29] = new List<string>();
            sokoban[25].Add("                "); sokoban[26].Add("                "); sokoban[27].Add("                "); sokoban[28].Add("                "); sokoban[29].Add("                ");
            sokoban[25].Add("       XXXXXX   "); sokoban[26].Add("    XXXXX       "); sokoban[27].Add("       XXXX     "); sokoban[28].Add("       XXXXX    "); sokoban[29].Add("    XXXXXXX     ");
            sokoban[25].Add("      XX'MMMX   "); sokoban[26].Add("    X'''XXXX    "); sokoban[27].Add(" XXXXXXX''X     "); sokoban[28].Add(" XXXX XX'''XXX  "); sokoban[29].Add("    X''X''X     ");
            sokoban[25].Add("     XX'C'X'X   "); sokoban[26].Add("    X'MOM''X    "); sokoban[27].Add(" X''C'''''X     "); sokoban[28].Add(" X''XXX''XCO'X  "); sokoban[29].Add("    X'''''X     ");
            sokoban[25].Add("     X'C'MMMX   "); sokoban[26].Add("   XXXMCX''X    "); sokoban[27].Add(" X''CX''''XXXX  "); sokoban[28].Add(" XMMM'''''CC'X  "); sokoban[29].Add("    XMMXCCX     ");
            sokoban[25].Add("   XXXC''X'XX   "); sokoban[26].Add("   X'CMCMCXX    "); sokoban[27].Add(" XXX''X'XXX''X  "); sokoban[28].Add(" XM'MXX'X'X''X  "); sokoban[29].Add("  XXX'''''XXX   ");
            sokoban[25].Add("   X'COX'''X    "); sokoban[26].Add("   X''MXM'XX    "); sokoban[27].Add("   X'COC''CC'X  "); sokoban[28].Add(" XMMMX''CCXCXX  "); sokoban[29].Add("  X'MMMXCCC'X   ");
            sokoban[25].Add("   X''C'X''X    "); sokoban[26].Add("   XXC'C'C'X    "); sokoban[27].Add("   X'CXX'XX''X  "); sokoban[28].Add(" XX''CCX'''''X  "); sokoban[29].Add("  X'''''''''X   ");
            sokoban[25].Add("   X'''C''XX    "); sokoban[26].Add("    X''X'''X    "); sokoban[27].Add("   X'''MMMM'XX  "); sokoban[28].Add("  XXX''''X'''X  "); sokoban[29].Add("  XXXMMXCCXXX   ");
            sokoban[25].Add("   XX''X''X     "); sokoban[26].Add("    XXXXXXXX    "); sokoban[27].Add("   XXXXMMM''X   "); sokoban[28].Add("    XXX''XXXXX  "); sokoban[29].Add("    X''O''X     ");
            sokoban[25].Add("    XXXXXXX     "); sokoban[26].Add("                "); sokoban[27].Add("      XXXXXXX   "); sokoban[28].Add("      XXXX      "); sokoban[29].Add("    XXXXXXX     ");
            sokoban[25].Add("                "); sokoban[26].Add("                "); sokoban[27].Add("                "); sokoban[28].Add("                "); sokoban[29].Add("                ");

            sokoban[30] = new List<string>(); sokoban[31] = new List<string>(); sokoban[32] = new List<string>(); sokoban[33] = new List<string>(); sokoban[34] = new List<string>();
            sokoban[30].Add("                "); sokoban[31].Add("                "); sokoban[32].Add("   XXXXXXXXX    "); sokoban[33].Add("                "); sokoban[34].Add("      XXXXX     ");
            sokoban[30].Add("   XXXXXXXX     "); sokoban[31].Add("                "); sokoban[32].Add("   X'''X'''X    "); sokoban[33].Add("       XXXXX    "); sokoban[34].Add("   XXXX'''XXX'  ");
            sokoban[30].Add("   X''X'''XXX   "); sokoban[31].Add("   XXXXXXX      "); sokoban[32].Add("   X'C'O'C'X    "); sokoban[33].Add("    XXXX'''XX   "); sokoban[34].Add("  XX''C'C'''XX  ");
            sokoban[30].Add("   X'''MCM''X   "); sokoban[31].Add("   XMMM''XXX    "); sokoban[32].Add("   XX'XXX'XX    "); sokoban[33].Add("   XX''CC'C'X   "); sokoban[34].Add("  X''XXX'XX''X  ");
            sokoban[30].Add("   X''XXOXX'X   "); sokoban[31].Add("   X'XX''''X    "); sokoban[32].Add("   X''MXM''X    "); sokoban[33].Add("   X''''MXC'X   "); sokoban[34].Add("  X'X''C'''X'X  ");
            sokoban[30].Add("   XX'CMCM''X   "); sokoban[31].Add("   X''C''C'X    "); sokoban[32].Add("   X'CMMMC'X    "); sokoban[33].Add("   X''MX''''X   "); sokoban[34].Add("  X'MMMMMMMM'X  ");
            sokoban[30].Add("   X''XC''X'X   "); sokoban[31].Add("   XMXCXXCXX    "); sokoban[32].Add("   XXCMXMCXX    "); sokoban[33].Add("  XXMXC'CM'XX   "); sokoban[34].Add("  X'X'CCC''X'X  ");
            sokoban[30].Add("   X''CMXMC'X   "); sokoban[31].Add("   X'''XO'X     "); sokoban[32].Add("   X''XXX''X    "); sokoban[33].Add("  X''''M'XXX    "); sokoban[34].Add("  X''XX'XXX''X  ");
            sokoban[30].Add("   XXXX'''XXX   "); sokoban[31].Add("   XXX'''XX     "); sokoban[32].Add("   X'''C'''X    "); sokoban[33].Add("  X'OMXXXX      "); sokoban[34].Add("  XX'C'''C''XX  ");
            sokoban[30].Add("      XXXXX     "); sokoban[31].Add("     XXXXX      "); sokoban[32].Add("   X''XXX''X    "); sokoban[33].Add("  XXXXX         "); sokoban[34].Add("   XXX'O'XXXX   ");
            sokoban[30].Add("                "); sokoban[31].Add("                "); sokoban[32].Add("   XXXX XXXX    "); sokoban[33].Add("                "); sokoban[34].Add("     XXXXX      ");
            sokoban[30].Add("                "); sokoban[31].Add("                "); sokoban[32].Add("                "); sokoban[33].Add("                "); sokoban[34].Add("                ");

            sokoban[35] = new List<string>(); sokoban[36] = new List<string>(); sokoban[37] = new List<string>(); sokoban[38] = new List<string>(); sokoban[39] = new List<string>();
            sokoban[35].Add("                "); sokoban[36].Add("                "); sokoban[37].Add("                "); sokoban[38].Add("                "); sokoban[39].Add("                ");
            sokoban[35].Add("                "); sokoban[36].Add("     XXXXX      "); sokoban[37].Add("                "); sokoban[38].Add("       XXXX     "); sokoban[39].Add("   XXXXXX       ");
            sokoban[35].Add("        XXXX    "); sokoban[36].Add("     X''OXX     "); sokoban[37].Add("     XXXXX      "); sokoban[38].Add("    XXXXO'X     "); sokoban[39].Add("   X'''MXXXXX   ");
            sokoban[35].Add("    XXXXXO'X    "); sokoban[36].Add("     X'CC'X     "); sokoban[37].Add("    XX'''XX     "); sokoban[38].Add("    X'C'''X     "); sokoban[39].Add("   X'X'M''''X   ");
            sokoban[35].Add("    X'C'CCCX    "); sokoban[36].Add("     XC'X'X     "); sokoban[37].Add("    X''X''X     "); sokoban[38].Add("   XX'XXXCXX    "); sokoban[39].Add("   X'X'MXCX'X   ");
            sokoban[35].Add("    X''MXM''    "); sokoban[36].Add("    XX'MMMX     "); sokoban[37].Add("   XX'MCM'XX    "); sokoban[38].Add("   XM''C''MX    "); sokoban[39].Add("   XM''MX'C'X   ");
            sokoban[35].Add("    XX'X''X'    "); sokoban[36].Add("    X''X'XX     "); sokoban[37].Add("   X'CCMCC'X    "); sokoban[38].Add("   X'''X'''X    "); sokoban[39].Add("   XX'XMO'X'X   ");
            sokoban[35].Add("     X'M'M''    "); sokoban[36].Add("    X''''X      "); sokoban[37].Add("   X'MCCCM'X    "); sokoban[38].Add("   XXX'X'XXX    "); sokoban[39].Add("    X'C'X'C'X   ");
            sokoban[35].Add("     X'''XXX    "); sokoban[36].Add("    XXX''X      "); sokoban[37].Add("   XXOMMM'XX    "); sokoban[38].Add("    XM'''X      "); sokoban[39].Add("    XXX''CC'X   ");
            sokoban[35].Add("     XXXXX      "); sokoban[36].Add("      XXXX      "); sokoban[37].Add("    XXXXXXX     "); sokoban[38].Add("    X'''XX      "); sokoban[39].Add("      XX'''XX   ");
            sokoban[35].Add("                "); sokoban[36].Add("                "); sokoban[37].Add("                "); sokoban[38].Add("    XXXXX       "); sokoban[39].Add("       XXXXX    ");
            sokoban[35].Add("                "); sokoban[36].Add("                "); sokoban[37].Add("                "); sokoban[38].Add("                "); sokoban[39].Add("                ");

            sokoban[40] = new List<string>(); sokoban[41] = new List<string>(); sokoban[42] = new List<string>(); sokoban[43] = new List<string>(); sokoban[44] = new List<string>();
            sokoban[40].Add("                "); sokoban[41].Add("  XXXXX  XXXX   "); sokoban[42].Add("                "); sokoban[43].Add("                "); sokoban[44].Add("                ");
            sokoban[40].Add("      XXXXX     "); sokoban[41].Add("  X'''XXXX''X   "); sokoban[42].Add("   XXXXXXXX     "); sokoban[43].Add("     XXXXXXX    "); sokoban[44].Add("                ");
            sokoban[40].Add("   XXXX'''X     "); sokoban[41].Add("  X''''''C'OX   "); sokoban[42].Add("   X'''X''XX    "); sokoban[43].Add("   XXX''X''XXX  "); sokoban[44].Add("    XXXX        ");
            sokoban[40].Add("   X''XMXCXXX   "); sokoban[41].Add("  XXCXXXXX''X   "); sokoban[42].Add("   X'''C'''X    "); sokoban[43].Add("   X''CMOMC''X  "); sokoban[44].Add("   XXO'XXXXX    ");
            sokoban[40].Add("   X'''M'C''X   "); sokoban[41].Add("  X''XMMMC'XX   "); sokoban[42].Add("   XX'XXMMCX    "); sokoban[43].Add("   X'X'MMM'X'X  "); sokoban[44].Add("   X'CCC'''X    ");
            sokoban[40].Add("   XMMMMXCX'X   "); sokoban[41].Add("  X''CM'MX''X   "); sokoban[42].Add("    XOXX'M'X    "); sokoban[43].Add("   X'C'XMX'C'X  "); sokoban[44].Add("   X'X'X'X'X    ");
            sokoban[40].Add("   XXXCX'C''X   "); sokoban[41].Add("  X''XMMMXXCXX  "); sokoban[42].Add("    X'M'XXCX    "); sokoban[43].Add("   XX''XMX''XX  "); sokoban[44].Add("   X'MMM'''X    ");
            sokoban[40].Add("    X'C''O'XX   "); sokoban[41].Add("  XXCXXXCXX''X  "); sokoban[42].Add("    XC'MXX'XX   "); sokoban[43].Add("    X'C'C'C'X   "); sokoban[44].Add("   XXXX'''XX    ");
            sokoban[40].Add("    X'''XXXX    "); sokoban[41].Add("  X'''''''C''X  "); sokoban[42].Add("    X'''C'''X   "); sokoban[43].Add("    X''XXX''X   "); sokoban[44].Add("      XXXXX     ");
            sokoban[40].Add("    XXXXX       "); sokoban[41].Add("  X'''XX''X''X  "); sokoban[42].Add("    XX''X'''X   "); sokoban[43].Add("    XXXX XXXX   "); sokoban[44].Add("                ");
            sokoban[40].Add("                "); sokoban[41].Add("  XXXXXXXXXXXX  "); sokoban[42].Add("     XXXXXXXX   "); sokoban[43].Add("                "); sokoban[44].Add("                ");
            sokoban[40].Add("                "); sokoban[41].Add("                "); sokoban[42].Add("                "); sokoban[43].Add("                "); sokoban[44].Add("                ");

            sokoban[45] = new List<string>(); sokoban[46] = new List<string>(); sokoban[47] = new List<string>(); sokoban[48] = new List<string>(); sokoban[49] = new List<string>();
            sokoban[45].Add("                "); sokoban[46].Add("                "); sokoban[47].Add("                "); sokoban[48].Add("                "); sokoban[49].Add("                ");
            sokoban[45].Add("     XXXXX      "); sokoban[46].Add("       XXXX     "); sokoban[47].Add("       XXXX     "); sokoban[48].Add("                "); sokoban[49].Add("  XXXXX XXXX    ");
            sokoban[45].Add("    XX'''XXXX   "); sokoban[46].Add("    XXXXM'XX    "); sokoban[47].Add("   XXXXX''X     "); sokoban[48].Add("      XXXXX     "); sokoban[49].Add("  X'''XXX''XX   ");
            sokoban[45].Add("   XX'CMC'''X   "); sokoban[46].Add("   XX''''''X    "); sokoban[47].Add("   X'''XC'X     "); sokoban[48].Add("   XXXX'''X     "); sokoban[49].Add("  X''''C''''X   ");
            sokoban[45].Add("   X''CMC'''X   "); sokoban[46].Add("   X''CXC''X    "); sokoban[47].Add("   X''''''XX    "); sokoban[48].Add("   X'C''XMXX    "); sokoban[49].Add("  XXCXXOX'XCX   ");
            sokoban[45].Add("  XX'M'O'XCXX   "); sokoban[46].Add("   XM'CMMX'XX   "); sokoban[47].Add("   XXCXXX''X    "); sokoban[48].Add("   X'''C'''X    "); sokoban[49].Add("   X'XMMMM''X   ");
            sokoban[45].Add("  X''XMX'MC'X   "); sokoban[46].Add("   XX'XMMC'MX   "); sokoban[47].Add("    X''MMM'X    "); sokoban[48].Add("   XXMXC'C'X    "); sokoban[49].Add("  XX''MMMMX'X   ");
            sokoban[45].Add("  X'''M'X'''X   "); sokoban[46].Add("    X'CCXC''X   "); sokoban[47].Add("   XXCX'MXXX    "); sokoban[48].Add("    X'O'XXXX    "); sokoban[49].Add("  X'CX'X'XXCXX  ");
            sokoban[45].Add("  XXX'C'C'XXX   "); sokoban[46].Add("    X'''OC'XX   "); sokoban[47].Add("   X''XXOXX     "); sokoban[48].Add("    XXXXX       "); sokoban[49].Add("  X''C'C'C'''X  ");
            sokoban[45].Add("    XXXX''X     "); sokoban[46].Add("    XX'MXXXX    "); sokoban[47].Add("   X''C'''X     "); sokoban[48].Add("                "); sokoban[49].Add("  X''XXXXX'''X  ");
            sokoban[45].Add("       XXXX     "); sokoban[46].Add("     XXXX       "); sokoban[47].Add("   XXXXXXXX     "); sokoban[48].Add("                "); sokoban[49].Add("  XXXX   XXXXX  ");
            sokoban[45].Add("                "); sokoban[46].Add("                "); sokoban[47].Add("                "); sokoban[48].Add("                "); sokoban[49].Add("                ");

            sokoban[50] = new List<string>(); sokoban[51] = new List<string>(); sokoban[52] = new List<string>(); sokoban[53] = new List<string>(); sokoban[54] = new List<string>();
            sokoban[50].Add("                "); sokoban[51].Add("                "); sokoban[52].Add("                "); sokoban[53].Add("                "); sokoban[54].Add(" XXXXX          ");
            sokoban[50].Add("      XXXXX     "); sokoban[51].Add("  XXXXX         "); sokoban[52].Add("      XXXXX     "); sokoban[53].Add("                "); sokoban[54].Add(" X'''XXXXXXXXX  ");
            sokoban[50].Add("   XXXX'''X     "); sokoban[51].Add("  X'''XXXXXXX   "); sokoban[52].Add("      X'''XX    "); sokoban[53].Add("    XXXXXXXX    "); sokoban[54].Add(" X'C''''XX'''X  ");
            sokoban[50].Add("   X''''C'X     "); sokoban[51].Add("  X'C''''X''X   "); sokoban[52].Add("      X''''X    "); sokoban[53].Add("    X''M'''X    "); sokoban[54].Add(" XXCXMMM'''''X  ");
            sokoban[50].Add("   X'''XCXXX    "); sokoban[51].Add("  XXCX'''CC'X   "); sokoban[52].Add("      XXCM'X    "); sokoban[53].Add("    XMCMMC'X    "); sokoban[54].Add(" XX'XXCXXXX'XX  ");
            sokoban[50].Add("   XX'XX'O'X    "); sokoban[51].Add("  X''XXMXX''X   "); sokoban[52].Add("    XXXOCMXX    "); sokoban[53].Add("   XXCCOCCXX    "); sokoban[54].Add(" X''XXMMMOX''X  ");
            sokoban[50].Add("   X'MMX'''X    "); sokoban[51].Add("  X'CCMMM''XX   "); sokoban[52].Add("   XX'MMMM'X    "); sokoban[53].Add("   X'CMMCMX     "); sokoban[54].Add(" X''C'''X'C''X  ");
            sokoban[50].Add("   X'MMCC'XX    "); sokoban[51].Add("  X''XMMMXOX    "); sokoban[52].Add("   X'''C'X'X    "); sokoban[53].Add("   X'''M''X     "); sokoban[54].Add(" X''XXXCXXX''X  ");
            sokoban[50].Add("   XX''X''X     "); sokoban[51].Add("  X'XXXX'XCX    "); sokoban[52].Add("   X'CCCXX'X    "); sokoban[53].Add("   XXXXXXXX     "); sokoban[54].Add(" XXXXX''''''XX  ");
            sokoban[50].Add("    XXXXXXX     "); sokoban[51].Add("  X''''''''X    "); sokoban[52].Add("   X''X''''X    "); sokoban[53].Add("                "); sokoban[54].Add("     X'''XXXX   ");
            sokoban[50].Add("                "); sokoban[51].Add("  XXXXXXXXXX    "); sokoban[52].Add("   XXXXXXXXX    "); sokoban[53].Add("                "); sokoban[54].Add("     XXXXX      ");
            sokoban[50].Add("                "); sokoban[51].Add("                "); sokoban[52].Add("                "); sokoban[53].Add("                "); sokoban[54].Add("                ");

            sokoban[55] = new List<string>(); sokoban[56] = new List<string>(); sokoban[57] = new List<string>(); sokoban[58] = new List<string>(); sokoban[59] = new List<string>();
            sokoban[55].Add("                "); sokoban[56].Add("    XXXX        "); sokoban[57].Add("                "); sokoban[58].Add("   XXXXX        "); sokoban[59].Add("       XXXXX    ");
            sokoban[55].Add("   XXXXXXXX     "); sokoban[56].Add("    X''XXXX     "); sokoban[57].Add("       XXXXX    "); sokoban[58].Add("   X'''XXXXXX   "); sokoban[59].Add("   XXXXX'''X    ");
            sokoban[55].Add("   X''X'''XXX   "); sokoban[56].Add("   XX''C''XXX   "); sokoban[57].Add("  XXXXXX'''XX   "); sokoban[58].Add("   X'XCXX'''X   "); sokoban[59].Add("   X'''XCX'X    ");
            sokoban[55].Add("  XX''C'''''X   "); sokoban[56].Add("   X''XXCMM'X   "); sokoban[57].Add("  X''''C'C''X   "); sokoban[58].Add("   X''''C'''X   "); sokoban[59].Add("   X'C''C''X    ");
            sokoban[55].Add("  X'''XXCX''X   "); sokoban[56].Add("   X'COX'M''X   "); sokoban[57].Add("  X'X'XXXCXOX   "); sokoban[58].Add("   XXXC'XXCXX   "); sokoban[59].Add("   XXCXOXX'X    ");
            sokoban[55].Add("  X'C'XMMX'XX   "); sokoban[56].Add("  XX'XCCCX'XX   "); sokoban[57].Add("  X'''X''C''XX  "); sokoban[58].Add("     X'MMM'X    "); sokoban[59].Add("   X'''''X'X    ");
            sokoban[55].Add("  XXXCXMMX'X    "); sokoban[56].Add("  X''M'X'''X    "); sokoban[57].Add("  XXMMMM'XCC'X  "); sokoban[58].Add("     XC'MXCX    "); sokoban[59].Add("   X'CXXCX'XX   ");
            sokoban[55].Add("    XOCMM''X    "); sokoban[56].Add("  X'MM'XX''X    "); sokoban[57].Add("   X'XMXXX'X'X  "); sokoban[58].Add("    XX'MMM'X    "); sokoban[59].Add("   X''X'MX''X   ");
            sokoban[55].Add("    XCX'X''X    "); sokoban[56].Add("  XXX'''''XX    "); sokoban[57].Add("   X''M''''''X  "); sokoban[58].Add("    X'CXMXCX    "); sokoban[59].Add("   XX'MMM'''X   ");
            sokoban[55].Add("    X'''XXXX    "); sokoban[56].Add("    XXXX''X     "); sokoban[57].Add("   XX'''XXXXXX  "); sokoban[58].Add("    X'''O''X    "); sokoban[59].Add("    XXXMMX''X   ");
            sokoban[55].Add("    XXXXX       "); sokoban[56].Add("       XXXX     "); sokoban[57].Add("    XXXXX       "); sokoban[58].Add("    X''X''XX    "); sokoban[59].Add("      XXXXXXX   ");
            sokoban[55].Add("                "); sokoban[56].Add("                "); sokoban[57].Add("                "); sokoban[58].Add("    XXXXXXX     "); sokoban[59].Add("                ");

            sokoban[60] = new List<string>(); sokoban[61] = new List<string>(); sokoban[62] = new List<string>(); sokoban[63] = new List<string>(); sokoban[64] = new List<string>();
            sokoban[60].Add("  XXXXX  XXXX   "); sokoban[61].Add("                "); sokoban[62].Add("                "); sokoban[63].Add("                "); sokoban[64].Add("                ");
            sokoban[60].Add("  X'''XXXX''X   "); sokoban[61].Add("    XXXXXXX     "); sokoban[62].Add("        XXXX    "); sokoban[63].Add("  XXXXXXXX      "); sokoban[64].Add("  XXXXX         ");
            sokoban[60].Add("  X''''''CC'X   "); sokoban[61].Add("    X''X''XXX   "); sokoban[62].Add("   XXXXXX''X    "); sokoban[63].Add("  X''XO''X      "); sokoban[64].Add("  X'''XXXXXXX   ");
            sokoban[60].Add("  XXCXXXXX''X   "); sokoban[61].Add("  XXX'CX'M''X   "); sokoban[62].Add("   X''XMM'CX    "); sokoban[63].Add("  X''C'''XXXX   "); sokoban[64].Add("  X''''C''''X   ");
            sokoban[60].Add("  X''XMM'C'XX   "); sokoban[61].Add("  X''CMCM'''X   "); sokoban[62].Add(" XXX''XMMC'X    "); sokoban[63].Add("  X'CXMXX'''X   "); sokoban[64].Add("  XX'XXMMM''X   ");
            sokoban[60].Add("  X''MMMXX''X   "); sokoban[61].Add("  X''MCMCMXXX   "); sokoban[62].Add(" X'C'C'MX'CXXX  "); sokoban[63].Add("  XX''MMM'X'X   "); sokoban[64].Add("  XXCXXCXXCXX   ");
            sokoban[60].Add("  X''XMM'XCOX   "); sokoban[61].Add("  XX'CMCMCX     "); sokoban[62].Add(" X''C'XMOC'C'X  "); sokoban[63].Add("   X'XXMM'C'X   "); sokoban[64].Add("  X''MMMXX'XX   ");
            sokoban[60].Add("  XXXX'''X''X   "); sokoban[61].Add("   X'MCMCOX     "); sokoban[62].Add(" XXX'CMMX''''X  "); sokoban[63].Add("  XXCXXMXCX'X   "); sokoban[64].Add("  X'''COC'''X   ");
            sokoban[60].Add("    XXCXXXX'XX  "); sokoban[61].Add("   XXXMCXXX     "); sokoban[62].Add("   XC'MMX''XXX  "); sokoban[63].Add("  X''C''C'''X   "); sokoban[64].Add("  XXXXXXX'''X   ");
            sokoban[60].Add("    X''''C'''X  "); sokoban[61].Add("     X''X       "); sokoban[62].Add("   X''XXXXXX    "); sokoban[63].Add("  X'''XXXXXXX   "); sokoban[64].Add("        XXXXX   ");
            sokoban[60].Add("    X'''XX'''X  "); sokoban[61].Add("     XXXX       "); sokoban[62].Add("   XXXX         "); sokoban[63].Add("  XXXXX         "); sokoban[64].Add("                ");
            sokoban[60].Add("    XXXXXXXXXX  "); sokoban[61].Add("                "); sokoban[62].Add("                "); sokoban[63].Add("                "); sokoban[64].Add("                ");

            sokoban[65] = new List<string>(); sokoban[66] = new List<string>(); sokoban[67] = new List<string>(); sokoban[68] = new List<string>(); sokoban[69] = new List<string>();
            sokoban[65].Add(" XXXXX          "); sokoban[66].Add("   XXXXXXXXX    "); sokoban[67].Add("  XXXXXX        "); sokoban[68].Add("      XXXX      "); sokoban[69].Add("   XXXXXXXXXX   ");
            sokoban[65].Add(" X'''XXXXXXXXX  "); sokoban[66].Add("   X'''X'''X    "); sokoban[67].Add("  X''''XXXXXXX  "); sokoban[68].Add("    XXX''XX     "); sokoban[69].Add("   X'''X''''X   ");
            sokoban[65].Add(" X'C'''''''''XX "); sokoban[66].Add("   X'X'X'''X    "); sokoban[67].Add("  X'XX'C'C'C'X  "); sokoban[68].Add("    X'CCM'XXX   "); sokoban[69].Add("   X'XCCCCC'X   ");
            sokoban[65].Add(" XXCXXX'X'CMMMX "); sokoban[66].Add("   X'MCMMCXX    "); sokoban[67].Add("  X''M'X'X'X'X  "); sokoban[68].Add("    X'MCMCC'X   "); sokoban[69].Add("   X''MXMX''X   ");
            sokoban[65].Add("  X'C'''XX'C'MX "); sokoban[66].Add("   X''CMMC'X    "); sokoban[67].Add("  XXXMMC''O''X  "); sokoban[68].Add("    XX''M'M'X   "); sokoban[69].Add("   X''MMM''XX   ");
            sokoban[65].Add(" XX'X'MMX''CXMX "); sokoban[66].Add("   XXXCXOX'X    "); sokoban[67].Add("    XMMX'XXXXX  "); sokoban[68].Add("   XXX'OX'XXX   "); sokoban[69].Add("   XXX'O'XXX    ");
            sokoban[65].Add(" XMMXXMXX''C''X "); sokoban[66].Add("    X'CM'''X    "); sokoban[67].Add("    XX'XCX      "); sokoban[68].Add("   X'M'M''XX    "); sokoban[69].Add("     XXXXX      ");
            sokoban[65].Add(" XM''C''XX'CXXX "); sokoban[66].Add("    X'''XXXX    "); sokoban[67].Add("     X'''X      "); sokoban[68].Add("   X'CCMCM'X    "); sokoban[69].Add("                ");
            sokoban[65].Add(" X''X'CCOX''X   "); sokoban[66].Add("    XXXXX       "); sokoban[67].Add("     XXXXX      "); sokoban[68].Add("   XXX'MCC'X    "); sokoban[69].Add("                ");
            sokoban[65].Add(" XXXXX'''XXXX   "); sokoban[66].Add("                "); sokoban[67].Add("                "); sokoban[68].Add("     XX''XXX    "); sokoban[69].Add("                ");
            sokoban[65].Add("     XXXXX      "); sokoban[66].Add("                "); sokoban[67].Add("                "); sokoban[68].Add("      XXXX      "); sokoban[69].Add("                ");
            sokoban[65].Add("                "); sokoban[66].Add("                "); sokoban[67].Add("                "); sokoban[68].Add("                "); sokoban[69].Add("                ");

            sokoban[70] = new List<string>(); sokoban[71] = new List<string>(); sokoban[72] = new List<string>(); sokoban[73] = new List<string>(); sokoban[74] = new List<string>();
            sokoban[70].Add(" XXXXX          "); sokoban[71].Add("                "); sokoban[72].Add("   XXXXXXX      "); sokoban[73].Add("        XXXX    "); sokoban[74].Add("                ");
            sokoban[70].Add("XX'''XX         "); sokoban[71].Add("  XXXXXXXXXXX   "); sokoban[72].Add("   X'''''X      "); sokoban[73].Add("        X''X    "); sokoban[74].Add("    XXXXXX      ");
            sokoban[70].Add("X'''''X  XXXXX  "); sokoban[71].Add("  X''''O''''X   "); sokoban[72].Add("   X'C'C'XXXX   "); sokoban[73].Add("  XXXXXXXC'X    "); sokoban[74].Add("    X''''XXXX   ");
            sokoban[70].Add("X'CCX'X XX'''XX "); sokoban[71].Add("  X'XXXCXXX'X   "); sokoban[72].Add("  XXX'XX'M''X   "); sokoban[73].Add("  X'M'C'C''X    "); sokoban[74].Add("    XMCMCMC'X   ");
            sokoban[70].Add("XX''X'XXX'''''X "); sokoban[71].Add("  X'X'MCM'X'X   "); sokoban[72].Add("  X'''XM'C''X   "); sokoban[73].Add("  X''M'MM'XX    "); sokoban[74].Add("   XXCMCMCM'X   ");
            sokoban[70].Add(" XOCC'''X'''''X "); sokoban[71].Add("  X'XC'M'CX'X   "); sokoban[72].Add("  X'O''MCMXXX   "); sokoban[73].Add("  XXCXXMXCX     "); sokoban[74].Add("   X'MC'OMC'X   ");
            sokoban[70].Add(" X''X'''X'X'XXX "); sokoban[71].Add("  X''CM'MC''X   "); sokoban[72].Add("  XXX'XMMCX     "); sokoban[73].Add("   X'MOMM'XXXX  "); sokoban[74].Add("   X'CM''CM'X   ");
            sokoban[70].Add(" XXXX'XXX'X'''X "); sokoban[71].Add("  XXXCM'MCXXX   "); sokoban[72].Add("  XX'C'X''X     "); sokoban[73].Add("  XXM'C'C''C'X  "); sokoban[74].Add("   X'MCMCMCXX   ");
            sokoban[70].Add("   XM''MMM''X'X "); sokoban[71].Add("    X''M''X     "); sokoban[72].Add("  X'''C'C'X     "); sokoban[73].Add("  X''XXXXX'''X  "); sokoban[74].Add("   X'CMCMCMX    ");
            sokoban[70].Add("   X''XXXXX'''X "); sokoban[71].Add("    XXXXXXX     "); sokoban[72].Add("  X'''XXM'X     "); sokoban[73].Add("  X'CX   XXXXX  "); sokoban[74].Add("   XXXX''''X    ");
            sokoban[70].Add("   X''X   XXXXX "); sokoban[71].Add("                "); sokoban[72].Add("  XXXXXXXXX     "); sokoban[73].Add("  X''X          "); sokoban[74].Add("      XXXXXX    ");
            sokoban[70].Add("   XXXX         "); sokoban[71].Add("                "); sokoban[72].Add("                "); sokoban[73].Add("  XXXX          "); sokoban[74].Add("                ");

            sokoban[75] = new List<string>(); sokoban[76] = new List<string>(); sokoban[77] = new List<string>(); sokoban[78] = new List<string>(); sokoban[79] = new List<string>();
            sokoban[75].Add("                "); sokoban[76].Add("                "); sokoban[77].Add("                "); sokoban[78].Add("                "); sokoban[79].Add("                ");
            sokoban[75].Add("    XXXXXXXX    "); sokoban[76].Add("   XXXXXXXXX    "); sokoban[77].Add("   XXXXXXXXXX   "); sokoban[78].Add("     XXXX       "); sokoban[79].Add("        XXXXX   ");
            sokoban[75].Add("   XX''O'''XX   "); sokoban[76].Add("   X'''''''XX   "); sokoban[77].Add("   X'''MC'''X   "); sokoban[78].Add("  XXXX''X       "); sokoban[79].Add("   XXXXXX'''X   ");
            sokoban[75].Add("   X'XCMCMX'X   "); sokoban[76].Add("   X'CMCMCM'X   "); sokoban[77].Add("   X''MCMC''X   "); sokoban[78].Add("  X'''''XXXX    "); sokoban[79].Add("   X''C'''''X   ");
            sokoban[75].Add("   X'CMCMCM'X   "); sokoban[76].Add("   X'MCMCMC'X   "); sokoban[77].Add("   X'MCMCMC'X   "); sokoban[78].Add("  X'C'X''M'XX   "); sokoban[79].Add("   X''CXXX'XX   ");
            sokoban[75].Add("   X'MC''MC'X   "); sokoban[76].Add("   X'CMXXCM'X   "); sokoban[77].Add("   XMCMXXCMCX   "); sokoban[78].Add("  X''X'''M''X   "); sokoban[79].Add("   XXMCM'M'MX   ");
            sokoban[75].Add("   X'CM''CM'X   "); sokoban[76].Add("   X'MCXXMC'X   "); sokoban[77].Add("   XCMCXXMCMX   "); sokoban[78].Add("  XX'XCCXM''X   "); sokoban[79].Add("    X'CX''''X   ");
            sokoban[75].Add("   X'MCMCMC'X   "); sokoban[76].Add("   X'CMCMCM'X   "); sokoban[77].Add("   X'CMCMCM'X   "); sokoban[78].Add("  XX''''XXXXX   "); sokoban[79].Add("    X'OXXXXXX   ");
            sokoban[75].Add("   X'XMCMCX'X   "); sokoban[76].Add("   X'MCMCMC'X   "); sokoban[77].Add("   X''CMCM''X   "); sokoban[78].Add("  X'O'XXX       "); sokoban[79].Add("    X''X        ");
            sokoban[75].Add("   XX''''''XX   "); sokoban[76].Add("   XX''''''OX   "); sokoban[77].Add("   X'''CM''OX   "); sokoban[78].Add("  X'''X         "); sokoban[79].Add("    XXXX        ");
            sokoban[75].Add("    XXXXXXXX    "); sokoban[76].Add("    XXXXXXXXX   "); sokoban[77].Add("   XXXXXXXXXX   "); sokoban[78].Add("  XXXXX         "); sokoban[79].Add("                ");
            sokoban[75].Add("                "); sokoban[76].Add("                "); sokoban[77].Add("                "); sokoban[78].Add("                "); sokoban[79].Add("                ");

            sokoban[80] = new List<string>(); sokoban[81] = new List<string>(); sokoban[82] = new List<string>(); sokoban[83] = new List<string>(); sokoban[84] = new List<string>();
            sokoban[80].Add("                "); sokoban[81].Add("                "); sokoban[82].Add("                "); sokoban[83].Add("                "); sokoban[84].Add("                ");
            sokoban[80].Add("       XXXXX    "); sokoban[81].Add("  XXXXXXXXXXX   "); sokoban[82].Add("     XXXXXXXX   "); sokoban[83].Add("      XXXXX     "); sokoban[84].Add(" XXXX           ");
            sokoban[80].Add("    XXXX'''X    "); sokoban[81].Add("  XO''X''X''X   "); sokoban[82].Add("    XXMMMM'OX   "); sokoban[83].Add(" XXXX X'''XXXXX "); sokoban[84].Add(" X''XXXXXXXXX   ");
            sokoban[80].Add("    X'O'CX'X    "); sokoban[81].Add("  X''CXC'''CX   "); sokoban[82].Add("    X''X'M''X   "); sokoban[83].Add(" X''XXXCX''X''X "); sokoban[84].Add(" X''''XX''''X   ");
            sokoban[80].Add("    X'XMMMMX    "); sokoban[81].Add("  XX''XMMX''X   "); sokoban[82].Add("   XX'X''X'XX   "); sokoban[83].Add(" X''M''MM'MMC'X "); sokoban[84].Add(" X'CCCX'''''X   ");
            sokoban[80].Add("   XXC'C'C'X    "); sokoban[81].Add("   X''XMMX''X   "); sokoban[82].Add("   X''XC'X'X    "); sokoban[83].Add(" X''M'XXX''X''X "); sokoban[84].Add(" XXMMMX'XCCCX   ");
            sokoban[80].Add("   X''XXX'XX    "); sokoban[81].Add("   X''XMMX''XX  "); sokoban[82].Add("   X'C'''X'XX   "); sokoban[83].Add(" XXXX'XX''X'''X "); sokoban[84].Add("  XMMMX'XMMMX   ");
            sokoban[80].Add("   X''''''X     "); sokoban[81].Add("   XC'''CXC''X  "); sokoban[82].Add("   XXXC'XX''X   "); sokoban[83].Add("   XXC'X'XX'XXX "); sokoban[84].Add("  XCCC''XMMMXX  ");
            sokoban[80].Add("   XXXXX''X     "); sokoban[81].Add("   X''X''X'''X  "); sokoban[82].Add("     X'''CC'X   "); sokoban[83].Add("   X'C'CC'''X   "); sokoban[84].Add("  X''''XXCCC'X  ");
            sokoban[80].Add("       XXXX     "); sokoban[81].Add("   XXXXXXXXXXX  "); sokoban[82].Add("     X'''X''X   "); sokoban[83].Add("   X'O'''XXXX   "); sokoban[84].Add("  XXXXXXX'O''X  ");
            sokoban[80].Add("                "); sokoban[81].Add("                "); sokoban[82].Add("     XXXXXXXX   "); sokoban[83].Add("   XXXXXXX      "); sokoban[84].Add("        XXXXXX  ");
            sokoban[80].Add("                "); sokoban[81].Add("                "); sokoban[82].Add("                "); sokoban[83].Add("                "); sokoban[84].Add("                ");

            sokoban[85] = new List<string>(); sokoban[86] = new List<string>(); sokoban[87] = new List<string>(); sokoban[88] = new List<string>(); sokoban[89] = new List<string>();
            sokoban[85].Add("                "); sokoban[86].Add("    XXXXX XXXXX "); sokoban[87].Add("   XXXXXXXXX    "); sokoban[88].Add("                "); sokoban[89].Add("                ");
            sokoban[85].Add("  XXXX  XXXX    "); sokoban[86].Add("    X'O'XXX'''X "); sokoban[87].Add("  XX'''''''XX   "); sokoban[88].Add("    XXXXXXX     "); sokoban[89].Add("      XXXXX     ");
            sokoban[85].Add("  X''XXXX''XXXX "); sokoban[86].Add("    X'X'C'''''X "); sokoban[87].Add("  X''X'X'X''X   "); sokoban[88].Add("    X''O''X     "); sokoban[89].Add("      X'''X     ");
            sokoban[85].Add("XXX'''''C'C'''X "); sokoban[86].Add(" XXXX'XC'C'XXXX "); sokoban[87].Add("  X'XMCMCMX'X   "); sokoban[88].Add("    X'CMC'X     "); sokoban[89].Add("  XXXXX'X'XXXX  ");
            sokoban[85].Add("X''''X'C''MMMMXX"); sokoban[86].Add(" X''''X'C'CX    "); sokoban[87].Add("  X''CMCMC''X   "); sokoban[88].Add("    XXMCMXX     "); sokoban[89].Add("  X'C'CC'C'C'X  ");
            sokoban[85].Add("X'C'XXXXXXCXMM'X"); sokoban[86].Add(" X'XXX'C'C'X    "); sokoban[87].Add("  X'XMCOMCX'X   "); sokoban[88].Add("    XX'C'XX     "); sokoban[89].Add("  XMMMMMMMMMMX  ");
            sokoban[85].Add("XX'''X'''''''''X"); sokoban[86].Add(" XMMMMMMMX'XX   "); sokoban[87].Add("  X''CMCMC''X   "); sokoban[88].Add("    X'CMC'X     "); sokoban[89].Add("  X'C'C'CC'C'X  ");
            sokoban[85].Add(" X'CC'CCO'XMMMXX"); sokoban[86].Add(" XXXX'XX'X''X   "); sokoban[87].Add("  X'XMCMCMX'X   "); sokoban[88].Add("    X'MCM'X     "); sokoban[89].Add("  XXXX'X'XXXXX  ");
            sokoban[85].Add(" X'''X''XXXXXXX "); sokoban[86].Add("    X''''X''X   "); sokoban[87].Add("  X''X'X'X''X   "); sokoban[88].Add("    X'MCM'X     "); sokoban[89].Add("     X'O'X      ");
            sokoban[85].Add(" XXXX'''X       "); sokoban[86].Add("    XXXXXX''X   "); sokoban[87].Add("  XX'''''''XX   "); sokoban[88].Add("    X''X''X     "); sokoban[89].Add("     XXXXX      ");
            sokoban[85].Add("    XXXXX       "); sokoban[86].Add("         XXXX   "); sokoban[87].Add("   XXXXXXXXX    "); sokoban[88].Add("    XXXXXXX     "); sokoban[89].Add("                ");
            sokoban[85].Add("                "); sokoban[86].Add("                "); sokoban[87].Add("                "); sokoban[88].Add("                "); sokoban[89].Add("                ");

            sokoban[90] = new List<string>(); sokoban[91] = new List<string>(); sokoban[92] = new List<string>(); sokoban[93] = new List<string>(); sokoban[94] = new List<string>();
            sokoban[90].Add("      XXXX      "); sokoban[91].Add("    XXXXXXXX    "); sokoban[92].Add("       XXXXX    "); sokoban[93].Add("    XXXXX       "); sokoban[94].Add("  XXXXXXXXXXX   ");
            sokoban[90].Add("     XX''X      "); sokoban[91].Add("    X''''''X    "); sokoban[92].Add("    XXXX'''X    "); sokoban[93].Add("   XX'''XX      "); sokoban[94].Add("XXXM''MCM''MXXX ");
            sokoban[90].Add("   XXX'MCXX     "); sokoban[91].Add("  XXXMXXXXMX    "); sokoban[92].Add("    X''M'X'XXX  "); sokoban[93].Add("  XX'MC''XX     "); sokoban[94].Add(" XX'C''C''C'XX  ");
            sokoban[90].Add("   X'CCMC'X     "); sokoban[91].Add("  X''C'C'C'X    "); sokoban[92].Add("    X'CM'C'C'X  "); sokoban[93].Add("  X'MCMC'MXX    "); sokoban[94].Add("  XX'MMCMM'XX   ");
            sokoban[90].Add("   X'M'M''X     "); sokoban[91].Add("  X'XMMMMMMXXX  "); sokoban[92].Add("    X'XMXX'C'X  "); sokoban[93].Add("  X'CMCMC''XX   "); sokoban[94].Add("   XXCXCXCXX    ");
            sokoban[90].Add("   XX'CMCXXX    "); sokoban[91].Add("  X'XC'X'''''X  "); sokoban[92].Add("  XXXOXMMXC''X  "); sokoban[93].Add("  X''CMOMC''X   "); sokoban[94].Add("    XMC CMX     ");
            sokoban[90].Add("    XXCMC''X    "); sokoban[91].Add("  X'XM'XOX'X'X  "); sokoban[92].Add("  X'''XXMX''XX  "); sokoban[93].Add("  XX''CMCMC'X   "); sokoban[94].Add("    X''O''X     ");
            sokoban[90].Add("     X'M'M'X    "); sokoban[91].Add("  X'CCXX'XCX'X  "); sokoban[92].Add("  X'C'C'MX'CX   "); sokoban[93].Add("   XXM'CMCM'X   "); sokoban[94].Add("    XXX'XXX     ");
            sokoban[90].Add("     X'MCC'X    "); sokoban[91].Add("  X''C'''C'''X  "); sokoban[92].Add("  XX'XXXMXX'X   "); sokoban[93].Add("    XX''CM'XX   "); sokoban[94].Add("   XX'C'C'XX    ");
            sokoban[90].Add("     XXM'XXX    "); sokoban[91].Add("  X''XXXXXXXXX  "); sokoban[92].Add("   X''''''''X   "); sokoban[93].Add("     XX'''XX    "); sokoban[94].Add("   XM''C''MX    ");
            sokoban[90].Add("      XO'X      "); sokoban[91].Add("  XXXX          "); sokoban[92].Add("   XXXXX''XXX   "); sokoban[93].Add("      XXXXX     "); sokoban[94].Add("   XXX'M'XXX    ");
            sokoban[90].Add("      XXXX      "); sokoban[91].Add("                "); sokoban[92].Add("       XXXX     "); sokoban[93].Add("                "); sokoban[94].Add("     XXXXX      ");

            sokoban[95] = new List<string>(); sokoban[96] = new List<string>(); sokoban[97] = new List<string>(); sokoban[98] = new List<string>();
            sokoban[95].Add("  XXXXX XXXXX   "); sokoban[96].Add("                "); sokoban[97].Add("   XXXXXXXX     "); sokoban[98].Add("     XXXXX      ");
            sokoban[95].Add("  X'''XXX'''X   "); sokoban[96].Add("    XXXXX       "); sokoban[97].Add("   X'''O''XXX   "); sokoban[98].Add("    XX'''XX     ");
            sokoban[95].Add("  X'O'''''C'X   "); sokoban[96].Add("    X'''XX      "); sokoban[97].Add("  XX'XX'X'''X   "); sokoban[98].Add("   XX'MCM'XX    ");
            sokoban[95].Add("  XXCXXCCXCXX   "); sokoban[96].Add("    X'CM'X      "); sokoban[97].Add("  X''XX'''C'X   "); sokoban[98].Add("  XX''CMC''XX   ");
            sokoban[95].Add("   X''XMMX''X   "); sokoban[96].Add("    XXCM'X      "); sokoban[97].Add("  X'CXXCXX'XX   "); sokoban[98].Add("  X'MCMCMCM'X   ");
            sokoban[95].Add("   XC'XMMM''X   "); sokoban[96].Add("     X'M'X      "); sokoban[97].Add("  X'C'MCMCCX    "); sokoban[98].Add("  X'CMCOCMC'X   ");
            sokoban[95].Add("   X''XMMX''X   "); sokoban[96].Add("     XCMCXXX    "); sokoban[97].Add("  XXCMMMMM'X    "); sokoban[98].Add("  X'MCMCMCM'X   ");
            sokoban[95].Add("   XX'XXCXXXX   "); sokoban[96].Add("     XOMC''X    "); sokoban[97].Add("   X'MXCXMCX    "); sokoban[98].Add("  XX''CMCC'XXX  ");
            sokoban[95].Add("   X''XX''X     "); sokoban[96].Add("     XCMCM'X    "); sokoban[97].Add("   X'MMCMM'X    "); sokoban[98].Add("   XX'MCM''''X  ");
            sokoban[95].Add("   X''''''X     "); sokoban[96].Add("     X'''XXX    "); sokoban[97].Add("   XXXXX'CCX    "); sokoban[98].Add("    XX'''X'X'X  ");
            sokoban[95].Add("   X''XX''X     "); sokoban[96].Add("     XXXXX      "); sokoban[97].Add("       X'''X    "); sokoban[98].Add("     XXXXX'''X  ");
            sokoban[95].Add("   XXXXXXXX     "); sokoban[96].Add("                "); sokoban[97].Add("       XXXXX    "); sokoban[98].Add("         XXXXX  ");
        }

    }
}
