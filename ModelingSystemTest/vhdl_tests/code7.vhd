entity some_test_bench is
end entity;

architecture some_test_bench of some_test_bench is
	
begin

	compute_xor: process begin
		for I in FILE_OPEN_KIND loop
			report (I);
		end loop;  
	  wait;
	end process;
end architecture some_test_bench;