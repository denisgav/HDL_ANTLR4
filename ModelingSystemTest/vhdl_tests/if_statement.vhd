entity some_test_bench is
end entity;

architecture some_test_bench of some_test_bench is
		variable x : positive := 12;
		variable y : positive := 2;
		variable z : positive := 1;
begin

	compute_xor: process
	begin
		if x>y then
			if x>z then
				report x;
			else
				report z;
			end if;
		else
			if y>z then
				report y;
			end if;
		end if;
	wait;
	end process;

end architecture some_test_bench;

