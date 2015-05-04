BR L0
L0:
PUSH D0
ADD SP #2 SP
SUB SP #3 D0
PUSH #"This is test 10 for B level"
WRTS
PUSH #""
WRTLNS
PUSH #"This will test the immutability of i as well as whether for loops"
WRTS
PUSH #""
WRTLNS
PUSH #"Calculate their conditions once, then run that number of times\n"
WRTS
PUSH #""
WRTLNS
PUSH #5
POP 2(D0)
PUSH #0
POP 1(D0)
ADD SP #1 SP
PUSH 2(D0)
POP 3(D0)
L1:
PUSH 3(D0)
PUSH 1(D0)
CMPGES
BRFS L2
PUSH #"i is "
WRTS
PUSH 1(D0)
WRTS
PUSH #" (i should not be changed, and this should show up 5 times)"
WRTS
PUSH #""
WRTLNS
PUSH #1
PUSH 1(D0)
ADDS
POP 1(D0)
BR L1
L2:
ADD SP #-1 SP
PUSH #""
WRTS
PUSH #""
WRTLNS
PUSH #0
POP 1(D0)
ADD SP #1 SP
PUSH 2(D0)
POP 3(D0)
L3:
PUSH 3(D0)
PUSH 1(D0)
CMPGES
BRFS L4
PUSH #1
POP 2(D0)
PUSH #"i is "
WRTS
PUSH 1(D0)
WRTS
PUSH #""
WRTLNS
PUSH #1
PUSH 1(D0)
ADDS
POP 1(D0)
BR L3
L4:
ADD SP #-1 SP
PUSH #"\ni should have made it to 5"
WRTS
PUSH #""
WRTLNS
SUB SP #2 SP
POP D0
HLT
