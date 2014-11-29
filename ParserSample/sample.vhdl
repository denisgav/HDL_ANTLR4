--library IEEE;
--using IEEE.std_logic_1164.all;

entity Mux8to1 is
generic(
	a : integer := 10;
);
port (
     Inputs : in bit;
     Select_s : in bit;
     Output : out bit
     );
end Mux8to1;

--architecture some_test_bench of some_test_bench is
--	subtype  my_real is real range 0.0 to 1024.0; 
--begin
--
--	sample_process: process
--		variable r1 : my_real := 550.0;
--		variable r2 : my_real := 220.5;
--		variable r3 : my_real := 0.1;
--	begin
--	  report(r1);
--	  report(r2);
--	  
--	  report(r3);--
--	  report(r3);--
--
--	  report(r1 + r2);
--
--	  r3 := (r1 + r2);
--	  report(r3);
--
--	  r3 := (r1 + r2)/3.2 - 1000.0;
--	  report(r3);
--	  
--	  wait;
--	end process;
--
--end architecture some_test_bench;
