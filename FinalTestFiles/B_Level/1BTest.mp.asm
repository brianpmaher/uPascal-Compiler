BR L0
L0:
PUSH D0
ADD SP #1 SP
SUB SP #2 D0
PUSH #"This is test 2 of B level"
WRTS
PUSH #""
WRTLNS
PUSH #"This should not compile"
WRTS
PUSH #""
WRTLNS
PUSH #1
PUSH #0
