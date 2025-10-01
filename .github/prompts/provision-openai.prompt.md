---
mode: agent
description: Provision a new Azure OpenAI resource with gpt-4o-mini and configure TranslationAgent.
model: Claude Sonnet 4 (copilot)
---

# Provision Azure OpenAI for TranslationAgent

You are an expert in Azure infrastructure automation and security best practices.

## Task

1. **Provision Resources**
   - Create a new **resource group** with a random, unique name.  
   - Create a new **Azure OpenAI resource** with a random, unique name.  
   - Deploy a **gpt-4o-mini** model to that resource instance.  

2. **Configuration**
   - Set the `AZURE_OPENAI_ENDPOINT` user secret in the `TranslationAgent` project using `dotnet user-secrets`.  
   - Use **managed identity only** — no static keys or secrets.  

3. **Implementation Rules**
   - Prefer **Azure CLI** commands (`az group create`, `az cognitiveservices account create`, `az cognitiveservices account deployment create`).  
   - If IaC is absolutely required, generate **minimal Bicep templates** and show how to run them with `az deployment`.  
   - Keep everything runnable via CLI in a developer environment.  

4. **Output Format**
   - Show CLI commands in **exact execution order**, without short inline comments.  
   - Do not include any keys, connection strings, or secrets.  
   - Use secure defaults and idempotent-friendly flags.  

5. **Assumptions**
   - You have the Azure CLI installed and are logged in.  
   - You have the `TranslationAgent` project set up with `dotnet user-secrets` initialized.  
   - You have permissions to create resources in your Azure subscription.
   - No changes to any code needs to be made. Just the call to `dotnet user-secrets` to set the endpoint.