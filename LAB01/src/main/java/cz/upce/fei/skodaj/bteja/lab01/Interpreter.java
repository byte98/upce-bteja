/*
 * Copyright (C) 2022 Jiri Skoda <jiri.skoda@student.upce.cz>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */
package cz.upce.fei.skodaj.bteja.lab01;
import java.util.*;

/**
 * 
 * @author Jiri Skoda <jiri.skoda@student.upce.cz>
 */
public class Interpreter
{
    char[] input;
    
    StringBuilder temp;
    
    int cursor;
    
    private Stack<Integer> stack;
    
    private static enum enumPlusMinusNothing
    {
        PLUS,
        MINUS,
        NOTHING
    };
    
    private static enum enumMultiplyDivideNothing
    {
        MULTIPLY,
        DIVIDE,
        NOTHING
    }
    
    public Interpreter(String input)
    {
        this.input = input.toCharArray();
        this.cursor = 0;
        this.stack = new Stack<>();
        this.temp = new StringBuilder();
    }
    
    private boolean loadExpression()
    {
        
    }
    
    private boolean loadTerm()
    {
        
    }
    
    private boolean loadFactor()
    {
        
    }
    
    private boolean loadNumber()
    {
        while (Character.isDigit(this.input[this.cursor]) && this.cursor < this.input.length)
        {
            this.temp.append(this.input[this.cursor]);
            this.cursor++;
        }
        if (this.temp.length() == 0)
        {
            return false;
        }
        else
        {
            this.stack.add()
        }
    }
    
    private enumPlusMinusNothing loadPlusMinus()
    {
        
    }
    
    private enumMultiplyDivideNothing loadMultiplyDivide()
    {
        
    }
}
