BR L0
L0:
PUSH D0
ADD SP #2 SP
SUB SP #3 D0
PUSH #"This is test 8 for B level"
WRTS
PUSH #""
WRTLNS
PUSH #"This will test repeat until loops"
WRTS
PUSH #""
WRTLNS
L1:
PUSH #"\nThis line should print once\n"
WRTS
PUSH #""
WRTLNS
PUSH #1
PUSH #2
CMPNES
BRFS L1
PUSH #5
POP 2(D0)
L2:
PUSH #"Please enter an integer"
WRTS
PUSH #""
WRTLNS
RD 1(D0)
PUSH 2(D0)
PUSH #5
CMPEQS
BRFS L2
L3:
PUSH 1(D0)
WRTS
PUSH #", "
WRTS
PUSH 1(D0)
PUSH #1
ADDS
POP 1(D0)
PUSH 1(D0)
PUSH 2(D0)
MODS
PUSH #0
CMPEQS
BRFS L3
PUSH #"\nShould have stopped one below the first multiple of 5 above your number"
WRTS
PUSH #""
WRTLNS
SUB SP #2 SP
POP D0
HLT
