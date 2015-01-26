# Begin asmlist al_begin
# End asmlist al_begin
# Begin asmlist al_stabs
# End asmlist al_stabs
# Begin asmlist al_procedures

.text
	.align 4
.globl	_PASCALMAIN
_PASCALMAIN:
# Temps allocated between ebp-12 and ebp+0
# [program1.pas]
# [5] begin {tester}
	pushl	%ebp
	movl	%esp,%ebp
	subl	$24,%esp
	movl	%ebx,-12(%ebp)
	movl	%esi,-8(%ebp)
	call	Lj2
Lj2:
	popl	%esi
	call	LFPC_INITIALIZEUNITS$stub
# [6] Writeln; Writeln;
	call	Lfpc_get_output$stub
	movl	%eax,%ebx
	movl	%ebx,%eax
	call	Lfpc_writeln_end$stub
	call	LFPC_IOCHECK$stub
	call	Lfpc_get_output$stub
	movl	%eax,%ebx
	movl	%ebx,%eax
	call	Lfpc_writeln_end$stub
	call	LFPC_IOCHECK$stub
# [7] Write('Please enter an integer value for I: ');
	call	Lfpc_get_output$stub
	movl	%eax,%ebx
	movl	%ebx,%edx
	movl	L_$TESTER$_Ld1$non_lazy_ptr-Lj2(%esi),%ecx
	movl	$0,%eax
	call	Lfpc_write_text_shortstr$stub
	call	LFPC_IOCHECK$stub
	movl	%ebx,%eax
	call	Lfpc_write_end$stub
	call	LFPC_IOCHECK$stub
# [8] Read(I);
	call	Lfpc_get_input$stub
	movl	%eax,%ebx
	leal	-4(%ebp),%edx
	movl	%ebx,%eax
	call	Lfpc_read_text_sint$stub
	call	LFPC_IOCHECK$stub
	movw	-4(%ebp),%ax
	movl	L_U_P$TESTER_I$non_lazy_ptr-Lj2(%esi),%edx
	movw	%ax,(%edx)
	movl	%ebx,%eax
	call	Lfpc_read_end$stub
	call	LFPC_IOCHECK$stub
# [9] I := I + 1;
	movl	L_U_P$TESTER_I$non_lazy_ptr-Lj2(%esi),%eax
	movswl	(%eax),%eax
	incl	%eax
	movl	L_U_P$TESTER_I$non_lazy_ptr-Lj2(%esi),%edx
	movw	%ax,(%edx)
# [10] Writeln('The current value of I is ', I:0);
	call	Lfpc_get_output$stub
	movl	%eax,%ebx
	movl	%ebx,%edx
	movl	L_$TESTER$_Ld2$non_lazy_ptr-Lj2(%esi),%ecx
	movl	$0,%eax
	call	Lfpc_write_text_shortstr$stub
	call	LFPC_IOCHECK$stub
	movl	L_U_P$TESTER_I$non_lazy_ptr-Lj2(%esi),%eax
	movswl	(%eax),%ecx
	movl	%ebx,%edx
	movl	$0,%eax
	call	Lfpc_write_text_sint$stub
	call	LFPC_IOCHECK$stub
	movl	%ebx,%eax
	call	Lfpc_writeln_end$stub
	call	LFPC_IOCHECK$stub
# [11] Writeln; Writeln;
	call	Lfpc_get_output$stub
	movl	%eax,%ebx
	movl	%ebx,%eax
	call	Lfpc_writeln_end$stub
	call	LFPC_IOCHECK$stub
	call	Lfpc_get_output$stub
	movl	%eax,%ebx
	movl	%ebx,%eax
	call	Lfpc_writeln_end$stub
	call	LFPC_IOCHECK$stub
# [12] end. {tester}
	call	LFPC_DO_EXIT$stub
	movl	-12(%ebp),%ebx
	movl	-8(%ebp),%esi
	leave
	ret

.text
	.align 2
.globl	_main
_main:
	jmp	L_FPC_SYSTEMMAIN$stub
# End asmlist al_procedures
# Begin asmlist al_globals


	.align 1
# [3] var I: Integer;
.globl _U_P$TESTER_I
.data
.zerofill __DATA, __common, _U_P$TESTER_I, 2,1



.data
	.align 2
.globl	_THREADVARLIST_P$TESTER
_THREADVARLIST_P$TESTER:
	.long	0
# [13] 

.data
	.align 2
.globl	INITFINAL
INITFINAL:
	.long	1,0
	.long	_INIT$_SYSTEM
	.long	0

.data
	.align 2
.globl	FPC_THREADVARTABLES
FPC_THREADVARTABLES:
	.long	2
	.long	_THREADVARLIST_SYSTEM
	.long	_THREADVARLIST_P$TESTER

.data
	.align 2
.globl	FPC_RESOURCESTRINGTABLES
FPC_RESOURCESTRINGTABLES:
	.long	0

.data
	.align 2
.globl	FPC_WIDEINITTABLES
FPC_WIDEINITTABLES:
	.long	0

.section __TEXT, .fpc, regular, no_dead_strip
	.align 3
	.ascii	"FPC 2.6.2 [2013/02/03] for i386 - Darwin"

.data
	.align 2
.globl	__stklen
__stklen:
	.long	262144

.data
	.align 2
.globl	__heapsize
__heapsize:
	.long	0

.data
.globl	__fpc_valgrind
__fpc_valgrind:
	.byte	0

.data
	.align 2
.globl	FPC_RESLOCATION
FPC_RESLOCATION:
	.long	0
# End asmlist al_globals
# Begin asmlist al_const
# End asmlist al_const
# Begin asmlist al_typedconsts

.const
	.align 2
.globl	_$TESTER$_Ld1
_$TESTER$_Ld1:
	.ascii	"%Please enter an integer value for I: \000"

.const
	.align 2
.globl	_$TESTER$_Ld2
_$TESTER$_Ld2:
	.ascii	"\032The current value of I is \000"
# End asmlist al_typedconsts
# Begin asmlist al_rotypedconsts
# End asmlist al_rotypedconsts
# Begin asmlist al_threadvars
# End asmlist al_threadvars
# Begin asmlist al_imports

.section __IMPORT,__jump_table,symbol_stubs,self_modifying_code+pure_instructions,5

Lfpc_get_output$stub:
.indirect_symbol fpc_get_output
	hlt
	hlt
	hlt
	hlt
	hlt

.section __IMPORT,__jump_table,symbol_stubs,self_modifying_code+pure_instructions,5

Lfpc_writeln_end$stub:
.indirect_symbol fpc_writeln_end
	hlt
	hlt
	hlt
	hlt
	hlt

.section __IMPORT,__jump_table,symbol_stubs,self_modifying_code+pure_instructions,5

LFPC_IOCHECK$stub:
.indirect_symbol FPC_IOCHECK
	hlt
	hlt
	hlt
	hlt
	hlt

.section __IMPORT,__jump_table,symbol_stubs,self_modifying_code+pure_instructions,5

Lfpc_write_text_shortstr$stub:
.indirect_symbol fpc_write_text_shortstr
	hlt
	hlt
	hlt
	hlt
	hlt

.section __IMPORT,__jump_table,symbol_stubs,self_modifying_code+pure_instructions,5

Lfpc_write_end$stub:
.indirect_symbol fpc_write_end
	hlt
	hlt
	hlt
	hlt
	hlt

.section __IMPORT,__jump_table,symbol_stubs,self_modifying_code+pure_instructions,5

Lfpc_get_input$stub:
.indirect_symbol fpc_get_input
	hlt
	hlt
	hlt
	hlt
	hlt

.section __IMPORT,__jump_table,symbol_stubs,self_modifying_code+pure_instructions,5

Lfpc_read_text_sint$stub:
.indirect_symbol fpc_read_text_sint
	hlt
	hlt
	hlt
	hlt
	hlt

.section __IMPORT,__jump_table,symbol_stubs,self_modifying_code+pure_instructions,5

Lfpc_read_end$stub:
.indirect_symbol fpc_read_end
	hlt
	hlt
	hlt
	hlt
	hlt

.section __IMPORT,__jump_table,symbol_stubs,self_modifying_code+pure_instructions,5

Lfpc_write_text_sint$stub:
.indirect_symbol fpc_write_text_sint
	hlt
	hlt
	hlt
	hlt
	hlt

.section __IMPORT,__jump_table,symbol_stubs,self_modifying_code+pure_instructions,5

LFPC_INITIALIZEUNITS$stub:
.indirect_symbol FPC_INITIALIZEUNITS
	hlt
	hlt
	hlt
	hlt
	hlt

.section __IMPORT,__jump_table,symbol_stubs,self_modifying_code+pure_instructions,5

LFPC_DO_EXIT$stub:
.indirect_symbol FPC_DO_EXIT
	hlt
	hlt
	hlt
	hlt
	hlt

.section __IMPORT,__jump_table,symbol_stubs,self_modifying_code+pure_instructions,5

L_FPC_SYSTEMMAIN$stub:
.indirect_symbol _FPC_SYSTEMMAIN
	hlt
	hlt
	hlt
	hlt
	hlt
# End asmlist al_imports
# Begin asmlist al_exports
# End asmlist al_exports
# Begin asmlist al_resources
# End asmlist al_resources
# Begin asmlist al_rtti
# End asmlist al_rtti
# Begin asmlist al_dwarf_frame
# End asmlist al_dwarf_frame
# Begin asmlist al_dwarf_info
# End asmlist al_dwarf_info
# Begin asmlist al_dwarf_abbrev
# End asmlist al_dwarf_abbrev
# Begin asmlist al_dwarf_line
# End asmlist al_dwarf_line
# Begin asmlist al_picdata

.section __DATA, __nl_symbol_ptr,non_lazy_symbol_pointers
	.align 2
L_$TESTER$_Ld1$non_lazy_ptr:
.indirect_symbol _$TESTER$_Ld1
	.long	0

.section __DATA, __nl_symbol_ptr,non_lazy_symbol_pointers
	.align 2
L_U_P$TESTER_I$non_lazy_ptr:
.indirect_symbol _U_P$TESTER_I
	.long	0

.section __DATA, __nl_symbol_ptr,non_lazy_symbol_pointers
	.align 2
L_$TESTER$_Ld2$non_lazy_ptr:
.indirect_symbol _$TESTER$_Ld2
	.long	0
# End asmlist al_picdata
# Begin asmlist al_resourcestrings
# End asmlist al_resourcestrings
# Begin asmlist al_objc_data
# End asmlist al_objc_data
# Begin asmlist al_objc_pools
# End asmlist al_objc_pools
# Begin asmlist al_end
# End asmlist al_end
	.subsections_via_symbols

