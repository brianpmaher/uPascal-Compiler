BR L0
L0:
PUSH D0
ADD SP #0 SP
SUB SP #1 D0
PUSH #"This is test 1 of B level"
WRTS
PUSH #""
WRTLNS
PUSH #"This test will simply test your boolean logic"
WRTS
PUSH #""
WRTLNS
PUSH #""
WRTS
PUSH #""
WRTLNS
PUSH #1
BRFS L1
PUSH #"This line should be 1st"
WRTS
PUSH #""
WRTLNS
BR L2
L1:
L2:
PUSH #0
BRFS L3
PUSH #"This should not print"
WRTS
PUSH #""
WRTLNS
BR L4
L3:
PUSH #"This line should be 2nd"
WRTS
PUSH #""
WRTLNS
L4:
PUSH #1
PUSH #1
ANDS
BRFS L5
PUSH #"This line should be 3rd"
WRTS
PUSH #""
WRTLNS
BR L6
L5:
L6:
PUSH #1
PUSH #0
ORS
BRFS L7
PUSH #"This line should be 4th"
WRTS
PUSH #""
WRTLNS
BR L8
L7:
L8:
PUSH #0
PUSH #1
ANDS
BRFS L9
PUSH #"This should not print"
WRTS
PUSH #""
WRTLNS
BR L10
L9:
PUSH #"This line should be 5th"
WRTS
PUSH #""
WRTLNS
L10:
PUSH #0
PUSH #0
ORS
BRFS L11
PUSH #"This should not print"
WRTS
PUSH #""
WRTLNS
BR L12
L11:
PUSH #"This line should be 6th"
WRTS
PUSH #""
WRTLNS
L12:
PUSH #1
PUSH #0
ORS
PUSH #1
ANDS
BRFS L13
PUSH #"This line should be 7th"
WRTS
PUSH #""
WRTLNS
BR L14
L13:
PUSH #"This should not print"
WRTS
PUSH #""
WRTLNS
L14:
PUSH #0
PUSH #1
ANDS
PUSH #1
ORS
BRFS L15
PUSH #"This line should be 8th"
WRTS
PUSH #""
WRTLNS
BR L16
L15:
PUSH #"This should not print"
WRTS
PUSH #""
WRTLNS
L16:
PUSH #0
BRFS L17
PUSH #"This should not print"
WRTS
PUSH #""
WRTLNS
BR L18
L17:
PUSH #1
BRFS L19
PUSH #"This line should be 9th"
WRTS
PUSH #""
WRTLNS
BR L20
L19:
L20:
L18:
PUSH #0
BRFS L21
PUSH #"This should not print"
WRTS
PUSH #""
WRTLNS
BR L22
L21:
PUSH #0
BRFS L23
PUSH #"This should not print"
WRTS
PUSH #""
WRTLNS
BR L24
L23:
PUSH #"This line should be 10th"
WRTS
PUSH #""
WRTLNS
L24:
L22:
PUSH #1
BRFS L25
PUSH #"This line should be last"
WRTS
PUSH #""
WRTLNS
BR L26
L25:
PUSH #1
BRFS L27
PUSH #"This should not print"
WRTS
PUSH #""
WRTLNS
BR L28
L27:
PUSH #"This should not print"
WRTS
PUSH #""
WRTLNS
L28:
L26:
SUB SP #0 SP
POP D0
HLT
